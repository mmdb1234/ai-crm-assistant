using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.Webhooks;

public static class WhatsAppWebhookEndpoints
{
    public static IEndpointRouteBuilder MapWhatsAppWebhookEndpoints(
        this IEndpointRouteBuilder app)
    {
        // WhatsApp webhook verification (Meta required)
        app.MapGet("/webhooks/whatsapp/{userId:guid}", (
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
        .WithName("WhatsAppWebhookVerify");

        // Per-user WhatsApp incoming messages
        app.MapPost("/webhooks/whatsapp/{userId:guid}", async (
            Guid userId,
            WhatsAppWebhookPayload payload,
            IChatIngestionService ingestionService,
            Domain.AI_Assistans.Interfaces.IChatConnectionRepository connectionRepo) =>
        {
            var connection = await connectionRepo.GetByUserAndPlatformAsync(
                userId, Domain.AI_Assistans.Enums.ChatPlatform.WhatsApp);
            if (connection is null)
                return Results.NotFound();

            var companyId = connection.User.CompanyId;

            foreach (var entry in payload.Entry)
            foreach (var change in entry.Changes)
            {
                if (change.Value?.Messages is null) continue;

                foreach (var msg in change.Value.Messages)
                {
                    if (msg.Type != "text" || msg.Text?.Body is null) continue;

                    var senderName = change.Value.Metadata?.DisplayPhoneNumber
                                     ?? change.Value.Metadata?.PhoneNumberId;

                    ingestionService.Enqueue(new IncomingChatMessage(
                        userId,
                        companyId,
                        Domain.AI_Assistans.Enums.ChatPlatform.WhatsApp,
                        msg.From ?? "unknown",
                        senderName,
                        msg.Text.Body,
                        msg.Id
                    ));
                }
            }

            return Results.Ok();
        })
        .AllowAnonymous()
        .WithName("WhatsAppWebhook")
        .WithDisplayName("WhatsApp Webhook");

        return app;
    }
}

// Minimal WhatsApp Cloud API webhook DTOs
public record WhatsAppWebhookPayload
{
    public string? Object { get; init; }
    public List<WhatsAppEntry> Entry { get; init; } = [];
}

public record WhatsAppEntry
{
    public string? Id { get; init; }
    public List<WhatsAppChange> Changes { get; init; } = [];
}

public record WhatsAppChange
{
    public WhatsAppValue? Value { get; init; }
    public string? Field { get; init; }
}

public record WhatsAppValue
{
    public WhatsAppMetadata? Metadata { get; init; }
    public List<WhatsAppMessage>? Messages { get; init; }
}

public record WhatsAppMetadata
{
    public string? DisplayPhoneNumber { get; init; }
    public string? PhoneNumberId { get; init; }
}

public record WhatsAppMessage
{
    public string? From { get; init; }
    public string? Id { get; init; }
    public string? Type { get; init; }
    public WhatsAppText? Text { get; init; }
}

public record WhatsAppText
{
    public string? Body { get; init; }
}
