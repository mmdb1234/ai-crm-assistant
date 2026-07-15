using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.Users.ConnectTelegramBot;

public record ConnectTelegramBotRequest
{
    [Required, MaxLength(500)]
    public string BotToken { get; init; } = default!;
}

public record ConnectTelegramBotResponse
{
    public long Id { get; init; }
    public string? BotUsername { get; init; }
    public bool IsActive { get; init; }
    public DateTime ConnectedAt { get; init; }
}

public static class ConnectTelegramBotEndpoints
{
    public static IEndpointRouteBuilder MapConnectTelegramBotEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/users/{userId:guid}/telegram-bot", async (
            Guid userId,
            ConnectTelegramBotRequest request,
            IChatConnectionRepository connectionRepo,
            IAppDbContext context,
            ITelegramBotService telegramBotService,
            HttpContext httpContext,
            ClaimsPrincipal user) =>
        {
            var companyIdClaim = user.FindFirst("CompanyId")?.Value;
            if (companyIdClaim is null || !int.TryParse(companyIdClaim, out var companyId))
                return Results.Unauthorized();

            var dbUser = await context.Users.FindAsync(userId);
            if (dbUser is null || dbUser.CompanyId != companyId)
                return Results.NotFound(new { error = "User not found in your company" });

            var existing = await connectionRepo.GetByUserAndPlatformAsync(userId, ChatPlatform.Telegram);
            if (existing is not null)
                return Results.Conflict(new { error = "User already has an active Telegram bot connection" });

            var botInfo = await telegramBotService.GetBotInfoAsync(request.BotToken);
            if (botInfo is null || !botInfo.Ok)
                return Results.BadRequest(new { error = "Invalid bot token" });

            var connection = new Domain.AI_Assistans.Entities.ChatConnection
            {
                UserId = userId,
                Platform = ChatPlatform.Telegram,
                BotToken = request.BotToken,
                BotUsername = botInfo.Result?.Username,
                IsActive = true,
                ConnectedAt = DateTime.UtcNow
            };

            var created = await connectionRepo.CreateAsync(connection);

            var webhookUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/webhooks/telegram/{userId}";

            var webhookSet = await telegramBotService.SetWebhookAsync(request.BotToken, webhookUrl);
            if (!webhookSet)
                return Results.Problem("Bot registered but webhook setup failed. Check that the server is publicly accessible via HTTPS.");

            return Results.Created($"/users/{userId}/telegram-bot", new ConnectTelegramBotResponse
            {
                Id = created.Id,
                BotUsername = created.BotUsername,
                IsActive = created.IsActive,
                ConnectedAt = created.ConnectedAt
            });
        })
        .RequireAuthorization()
        .WithName("ConnectTelegramBot")
        .WithDisplayName("Connect Telegram Bot")
        .Produces<ConnectTelegramBotResponse>(201)
        .Produces(400)
        .Produces(401)
        .Produces(404)
        .Produces(409);

        return app;
    }
}
