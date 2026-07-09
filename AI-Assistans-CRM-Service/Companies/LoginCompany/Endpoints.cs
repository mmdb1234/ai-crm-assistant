

using Features.AI_Assistans.Dtos;
using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.Conversations.LoginCompany
{
    public static class LoginCompanyEndpoint
    {
        public static IEndpointRouteBuilder MapLoginCompanyEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", async (
                LoginRequestDto request, IAuthService authService) =>
            {
                try
                {
                    var response = await authService.CompanyLoginAsync(request);

                    return Results.Ok(response);
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Unauthorized();
                }
            })
            .AllowAnonymous()
            .WithName("LoginCompany");

            return app;
        }
    }
}
