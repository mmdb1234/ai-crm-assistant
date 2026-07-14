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
                _logger.LogError(ex, "Failed to process {Platform} message for user {UserId}",
                    incoming.Platform, incoming.UserId);
            }
        }
    }

    private async Task ProcessMessageAsync(IncomingChatMessage incoming, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var connectionRepo = scope.ServiceProvider.GetRequiredService<IChatConnectionRepository>();
        var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        // Find existing conversation from this sender
        var existingConversation = await connectionRepo.GetActiveConversationBySenderAsync(
            incoming.UserId, incoming.ExternalSenderId, incoming.Platform);

        Guid conversationId;
        if (existingConversation is not null)
        {
            conversationId = existingConversation.Id;
        }
        else
        {
            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = $"{incoming.Platform}: {incoming.ExternalSenderName ?? incoming.ExternalSenderId}",
                UserId = incoming.UserId,
                CompanyId = incoming.CompanyId,
                Description = $"Auto-created from {incoming.Platform} chat",
                ExternalSenderId = incoming.ExternalSenderId,
                ExternalPlatform = incoming.Platform
            };
            context.Conversations.Add(conversation);
            await context.SaveChangesAsync(ct);
            conversationId = conversation.Id;
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

        _logger.LogInformation(
            "Message from {Platform} sender [{Sender}] added to conversation {ConversationId} for user {UserId}",
            incoming.Platform, incoming.ExternalSenderId, conversationId, incoming.UserId);
    }
}
