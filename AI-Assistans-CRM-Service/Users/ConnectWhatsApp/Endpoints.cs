using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.Users.ConnectWhatsApp;

public record ConnectWhatsAppRequest
{
    [Required, MaxLength(100)]
    public string PhoneNumberId { get; init; } = default!;

    [Required, MaxLength(20)]
    public string BusinessPhone { get; init; } = default!;
}

public record ConnectWhatsAppResponse
{
    public long Id { get; init; }
    public string? PhoneNumberId { get; init; }
    public string? BusinessPhone { get; init; }
    public bool IsActive { get; init; }
    public DateTime ConnectedAt { get; init; }
}

public static class ConnectWhatsAppEndpoints
{
    public static IEndpointRouteBuilder MapConnectWhatsAppEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/users/{userId:guid}/whatsapp-bot", async (
            Guid userId,
            ConnectWhatsAppRequest request,
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

            var existing = await connectionRepo.GetByUserAndPlatformAsync(userId, ChatPlatform.WhatsApp);
            if (existing is not null)
                return Results.Conflict(new { error = "User already has an active WhatsApp connection" });

            var connection = new Domain.AI_Assistans.Entities.ChatConnection
            {
                UserId = userId,
                Platform = ChatPlatform.WhatsApp,
                PhoneNumberId = request.PhoneNumberId,
                BusinessPhone = request.BusinessPhone,
                IsActive = true,
                ConnectedAt = DateTime.UtcNow
            };

            var created = await connectionRepo.CreateAsync(connection);

            return Results.Created($"/users/{userId}/whatsapp-bot", new ConnectWhatsAppResponse
            {
                Id = created.Id,
                PhoneNumberId = created.PhoneNumberId,
                BusinessPhone = created.BusinessPhone,
                IsActive = created.IsActive,
                ConnectedAt = created.ConnectedAt
            });
        })
        .RequireAuthorization()
        .WithName("ConnectWhatsApp")
        .WithDisplayName("Connect WhatsApp")
        .Produces<ConnectWhatsAppResponse>(201)
        .Produces(400)
        .Produces(401)
        .Produces(404)
        .Produces(409);

        return app;
    }
}