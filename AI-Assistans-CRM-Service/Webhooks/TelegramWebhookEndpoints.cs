namespace AI_Assistans_CRM_Service.Webhooks;

public static class TelegramWebhookEndpoints
{
    public static IEndpointRouteBuilder MapTelegramWebhookEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/webhooks/telegram", async (
            TelegramUpdate update,
            Features.AI_Assistans.Services.IChatIngestionService ingestionService) =>
        {
            if (update.Message?.Text is null)
                return Results.Ok();

            var chatId = update.Message.Chat.Id.ToString();
            var username = update.Message.From?.Username ?? update.Message.From?.FirstName;

            ingestionService.Enqueue(new Features.AI_Assistans.Services.IncomingChatMessage(
                Domain.AI_Assistans.Enums.ChatPlatform.Telegram,
                chatId,
                username,
                update.Message.Text,
                update.Message.MessageId.ToString()
            ));

            return Results.Ok();
        })
        .AllowAnonymous()
        .WithName("TelegramWebhook")
        .WithDisplayName("Telegram Webhook")
        .Produces(200);

        return app;
    }
}

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
