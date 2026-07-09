

using Features.AI_Assistans.Dtos;
using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.Conversations.RefreshToken
{
    public static class RefreshTokenEndpoint
    {
        public static IEndpointRouteBuilder MapRefreshTokenEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/refresh", async (
                RefreshTokenDto request, IAuthService authService) =>
            {
                try
                {
                    var response = await authService.RefreshTokenAsync(request);

                    return Results.Ok(response);
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Unauthorized();
                }
            })
            .RequireAuthorization()
            .WithName("RefreshToken");

            return app;
        }
    }
}
