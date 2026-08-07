using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.Webhooks.Instagram;

public static class InstagramWebhookEndpoints
{
    public static IEndpointRouteBuilder MapInstagramWebhookEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Instagram webhook verification (Meta required)
        app.MapGet("/webhooks/instagram/{userId:guid}", (
            Guid userId,
            [FromQuery] string? hubMode,
            [FromQuery] string? hubVerifyToken,
            [FromQuery] string? hubChallenge) =>
        {
            if (hubMode == "subscribe" && hubVerifyToken is not null)
                return Results.Ok(hubChallenge);

            return Results.Forbid();
        })
        .AllowAnonymous()
        .WithName("InstagramWebhookVerify")
        .WithDisplayName("Instagram Webhook Verify");

        // Per-user Instagram incoming messages
        app.MapPost("/webhooks/instagram/{userId:guid}", async (
            Guid userId,
            InstagramWebhookPayload payload,
            IChatIngestionService ingestionService,
            Domain.AI_Assistans.Interfaces.IChatConnectionRepository connectionRepo) =>
        {
            var connection = await connectionRepo.GetByUserAndPlatformAsync(
                userId, Domain.AI_Assistans.Enums.ChatPlatform.Instagram);
            if (connection is null)
                return Results.NotFound();

            var companyId = connection.User.CompanyId;

            foreach (var entry in payload.Entry)
            foreach (var message in entry.Messaging)
            {
                if (message.Message?.Text is null) continue;

                var senderId = message.Sender?.Id ?? "unknown";

                ingestionService.Enqueue(new IncomingChatMessage(
                    userId,
                    companyId,
                    Domain.AI_Assistans.Enums.ChatPlatform.Instagram,
                    senderId,
                    senderId,
                    message.Message.Text,
                    message.Message.Mid
                ));
            }

            return Results.Ok();
        })
        .AllowAnonymous()
        .WithName("InstagramWebhook")
        .WithDisplayName("Instagram Webhook");

        return app;
    }
}

// Minimal Instagram Messaging API webhook DTOs
public record InstagramWebhookPayload
{
    public string? Object { get; init; }
    public List<InstagramEntry> Entry { get; init; } = [];
}

public record InstagramEntry
{
    public string? Id { get; init; }
    public long Time { get; init; }
    public List<InstagramMessaging> Messaging { get; init; } = [];
}

public record InstagramMessaging
{
    public InstagramSender? Sender { get; init; }
    public InstagramRecipient? Recipient { get; init; }
    public long Timestamp { get; init; }
    public InstagramMessagingMessage? Message { get; init; }
}

public record InstagramSender
{
    public string? Id { get; init; }
}

public record InstagramRecipient
{
    public string? Id { get; init; }
}

public record InstagramMessagingMessage
{
    public string? Mid { get; init; }
    public string? Text { get; init; }
}