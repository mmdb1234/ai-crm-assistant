using System.Threading.Channels;
using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.AI_Assistans.Services;

public class ChatIngestionService : BackgroundService, IChatIngestionService
{
    private readonly Channel<IncomingChatMessage> _channel = Channel.CreateBounded<IncomingChatMessage>(100);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ChatIngestionService> _logger;

    public ChatIngestionService(IServiceScopeFactory scopeFactory, ILogger<ChatIngestionService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public void Enqueue(IncomingChatMessage message)
    {
        _channel.Writer.TryWrite(message);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ChatIngestionService started");

        await foreach (var incoming in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await ProcessMessageAsync(incoming, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message from {Platform} [{ChatId}]",
                    incoming.Platform, incoming.ExternalChatId);
            }
        }
    }

    private async Task ProcessMessageAsync(IncomingChatMessage incoming, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var connectionRepo = scope.ServiceProvider.GetRequiredService<IChatConnectionRepository>();
        var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        var connection = await connectionRepo.GetByExternalIdAsync(incoming.ExternalChatId, incoming.Platform);
        if (connection is null)
        {
            _logger.LogWarning("No active connection found for {Platform} chat {ChatId}", incoming.Platform, incoming.ExternalChatId);
            return;
        }

        Guid conversationId;
        if (connection.ActiveConversationId is null)
        {
            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = $"{incoming.Platform} - {incoming.ExternalUsername ?? incoming.ExternalChatId}",
                UserId = connection.UserId,
                CompanyId = connection.User.CompanyId,
                Description = $"Auto-created from {incoming.Platform} chat"
            };
            context.Conversations.Add(conversation);
            await context.SaveChangesAsync(ct);

            conversationId = conversation.Id;
            await connectionRepo.UpdateConversationAsync(connection.Id, conversationId);
        }
        else
        {
            conversationId = connection.ActiveConversationId.Value;
        }

        var message = new Message
        {
            ConversationId = conversationId,
            Role = MessageRole.Customer,
            Content = incoming.Text,
            SentAt = DateTime.UtcNow,
            SourcePlatform = incoming.Platform,
            ExternalMessageId = incoming.ExternalMessageId
        };
        context.Messages.Add(message);
        await context.SaveChangesAsync(ct);

        _logger.LogInformation("Message from {Platform} [{ChatId}] ingested into conversation {ConversationId}",
            incoming.Platform, incoming.ExternalChatId, conversationId);
    }
}
