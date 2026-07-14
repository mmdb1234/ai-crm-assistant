using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.TelegramBots;

public record RegisterBotRequest
{
    [Required, MaxLength(500)]
    public string BotToken { get; init; } = default!;
}

public record BotResponse
{
    public long Id { get; init; }
    public string? BotUsername { get; init; }
    public bool IsActive { get; init; }
    public DateTime ConnectedAt { get; init; }
}

public static class RegisterBotEndpoint
{
    public static IEndpointRouteBuilder MapRegisterBotEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/users/me/telegram-bot", async (
            RegisterBotRequest request,
            IChatConnectionRepository connectionRepo,
            IAppDbContext context,
            ITelegramBotService telegramBotService,
            HttpContext httpContext,
            ClaimsPrincipal user) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var dbUser = await context.Users.FindAsync(userId);
            if (dbUser is null)
                return Results.NotFound(new { error = "User not found" });

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

            var requestHost = httpContext.Request.Host.Value;
            var scheme = httpContext.Request.Scheme;
            var webhookUrl = $"{scheme}://{requestHost}/webhooks/telegram/{userId}";

            await telegramBotService.SetWebhookAsync(request.BotToken, webhookUrl);

            return Results.Created($"/users/me/telegram-bot", new BotResponse
            {
                Id = created.Id,
                BotUsername = created.BotUsername,
                IsActive = created.IsActive,
                ConnectedAt = created.ConnectedAt
            });
        })
        .RequireAuthorization()
        .WithName("RegisterTelegramBot")
        .WithDisplayName("Register Telegram Bot")
        .Produces<BotResponse>(201)
        .Produces(400)
        .Produces(401)
        .Produces(404)
        .Produces(409);

        return app;
    }
}
