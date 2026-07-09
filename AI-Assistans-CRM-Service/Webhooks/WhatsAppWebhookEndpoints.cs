namespace AI_Assistans_CRM_Service.Webhooks;

public static class WhatsAppWebhookEndpoints
{
    public static IEndpointRouteBuilder MapWhatsAppWebhookEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Verification endpoint (required by Meta)
        app.MapGet("/webhooks/whatsapp", (
            [FromQuery] string? hubMode,
            [FromQuery] string? hubVerifyToken,
            [FromQuery] string? hubChallenge) =>
        {
            if (hubMode == "subscribe" && hubVerifyToken is not null)
            {
                return Results.Ok(hubChallenge);
            }
            return Results.Forbid();
        })
        .AllowAnonymous()
        .WithName("WhatsAppWebhookVerify")
        .WithDisplayName("WhatsApp Webhook Verification");

        // Incoming messages
        app.MapPost("/webhooks/whatsapp", async (
            WhatsAppWebhookPayload payload,
            Features.AI_Assistans.Services.IChatIngestionService ingestionService) =>
        {
            foreach (var entry in payload.Entry)
            {
                foreach (var change in entry.Changes)
                {
                    if (change.Value?.Messages is null) continue;

                    foreach (var msg in change.Value.Messages)
                    {
                        if (msg.Type != "text" || msg.Text?.Body is null) continue;

                        var phoneNumber = change.Value.Metadata?.DisplayPhoneNumber
                                         ?? change.Value.Metadata?.PhoneNumberId;

                        ingestionService.Enqueue(new Features.AI_Assistans.Services.IncomingChatMessage(
                            Domain.AI_Assistans.Enums.ChatPlatform.WhatsApp,
                            msg.From,
                            phoneNumber ?? msg.From,
                            msg.Text.Body,
                            msg.Id
                        ));
                    }
                }
            }

            return Results.Ok();
        })
        .AllowAnonymous()
        .WithName("WhatsAppWebhook")
        .WithDisplayName("WhatsApp Webhook")
        .Produces(200);

        return app;
    }
}

// Minimal DTOs for WhatsApp Cloud API webhook payload
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
