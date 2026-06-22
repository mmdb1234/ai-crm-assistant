
using System.Security.Claims;

namespace Features.AI_Assistans.Users.CreateUser;

public static class CreateUserEndpoint
{
    public static IEndpointRouteBuilder MapCreateUserEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (
            ClaimsPrincipal company,
            CreateUserRequest request,
            IAppDbContext context,
            CancellationToken cancellationToken) =>
        {
            var companyIdClaim = company.FindFirst("CompanyId")?.Value;

            if (string.IsNullOrEmpty(companyIdClaim))
                return Results.Unauthorized();

            var companyId = int.Parse(companyIdClaim);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CompanyId = companyId
            };

            await context.Users.AddAsync(
                user,
                cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Ok(new CreateUserResponse
            {
                Id = user.Id,
                UserName = user.Username
            });
        })
        .RequireAuthorization()
        .WithName("CreateUser");

        return app;
    }
}



