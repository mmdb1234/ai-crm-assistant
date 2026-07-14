using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.Webhooks;

public static class TelegramWebhookEndpoints
{
    public static IEndpointRouteBuilder MapTelegramWebhookEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Per-user Telegram bot webhook
        app.MapPost("/webhooks/telegram/{userId:guid}", async (
            Guid userId,
            TelegramUpdate update,
            IChatIngestionService ingestionService,
            Domain.AI_Assistans.Interfaces.IChatConnectionRepository connectionRepo) =>
        {
            if (update.Message?.Text is null)
                return Results.Ok();

            var connection = await connectionRepo.GetByUserAndPlatformAsync(
                userId, Domain.AI_Assistans.Enums.ChatPlatform.Telegram);
            if (connection is null)
                return Results.NotFound();

            var senderId = update.Message.From?.Id.ToString() ?? "unknown";
            var senderName = update.Message.From?.Username
                          ?? update.Message.From?.FirstName;

            var companyId = connection.User.CompanyId;

            ingestionService.Enqueue(new IncomingChatMessage(
                userId,
                companyId,
                Domain.AI_Assistans.Enums.ChatPlatform.Telegram,
                senderId,
                senderName,
                update.Message.Text,
                update.Message.MessageId.ToString()
            ));

            return Results.Ok();
        })
        .AllowAnonymous()
        .WithName("TelegramWebhook")
        .WithDisplayName("Telegram Webhook");

        return app;
    }
}

// Minimal Telegram API Update DTOs
public record TelegramUpdate
{
    public long UpdateId { get; init; }
    public TelegramMessage? Message { get; init; }
}

public record TelegramMessage
{
    public long MessageId { get; init; }
    public TelegramChat Chat { get; init; } = default!;
    public TelegramUser? From { get; init; }
    public string? Text { get; init; }
}

public record TelegramChat
{
    public long Id { get; init; }
}

public record TelegramUser
{
    public long Id { get; init; }
    public string? FirstName { get; init; }
    public string? Username { get; init; }
}
