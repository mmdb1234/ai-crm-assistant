using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.Users.ConnectInstagram;

public record ConnectInstagramRequest
{
    [Required, MaxLength(500)]
    public string AccessToken { get; init; } = default!;

    [Required, MaxLength(100)]
    public string InstagramBusinessAccountId { get; init; } = default!;

    [MaxLength(200)]
    public string? InstagramUsername { get; init; }
}

public record ConnectInstagramResponse
{
    public long Id { get; init; }
    public string? InstagramUsername { get; init; }
    public bool IsActive { get; init; }
    public DateTime ConnectedAt { get; init; }
}

public static class ConnectInstagramEndpoints
{
    public static IEndpointRouteBuilder MapConnectInstagramEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/users/{userId:guid}/instagram-bot", async (
            Guid userId,
            ConnectInstagramRequest request,
            IChatConnectionRepository connectionRepo,
            IAppDbContext context,
            ClaimsPrincipal user) =>
        {
            var companyIdClaim = user.FindFirst("CompanyId")?.Value;
            if (companyIdClaim is null || !int.TryParse(companyIdClaim, out var companyId))
                return Results.Unauthorized();

            var dbUser = await context.Users.FindAsync(userId);
            if (dbUser is null || dbUser.CompanyId != companyId)
                return Results.NotFound(new { error = "User not found in your company" });

            var existing = await connectionRepo.GetByUserAndPlatformAsync(userId, ChatPlatform.Instagram);
            if (existing is not null)
                return Results.Conflict(new { error = "User already has an active Instagram connection" });

            var connection = new Domain.AI_Assistans.Entities.ChatConnection
            {
                UserId = userId,
                Platform = ChatPlatform.Instagram,
                // Reuse BotToken to store the Meta/Instagram access token
                BotToken = request.AccessToken,
                // Reuse PhoneNumberId to store the Instagram business account id
                PhoneNumberId = request.InstagramBusinessAccountId,
                // Reuse BotUsername/BusinessPhone to store the Instagram handle
                BusinessPhone = request.InstagramUsername,
                IsActive = true,
                ConnectedAt = DateTime.UtcNow
            };

            var created = await connectionRepo.CreateAsync(connection);

            return Results.Created($"/users/{userId}/instagram-bot", new ConnectInstagramResponse
            {
                Id = created.Id,
                InstagramUsername = created.BusinessPhone,
                IsActive = created.IsActive,
                ConnectedAt = created.ConnectedAt
            });
        })
        .RequireAuthorization()
        .WithName("ConnectInstagram")
        .WithDisplayName("Connect Instagram")
        .Produces<ConnectInstagramResponse>(201)
        .Produces(400)
        .Produces(401)
        .Produces(404)
        .Produces(409);

        return app;
    }
}