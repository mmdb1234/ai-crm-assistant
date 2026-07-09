using Features.AI_Assistans.Dtos;
using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service.Companies.RegisterCompany;

public static class RegisterCompanyEndpoint
{
    public static IEndpointRouteBuilder MapRegisterCompanyEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/register", async (
            RegisterRequestDto request,
            IAuthService authService) =>
        {
            try
            {
                var response = await authService.CompanyRegisterAsync(request);
                return Results.Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        })
        .AllowAnonymous()
        .WithName("RegisterCompany");

        return app;
    }
}
