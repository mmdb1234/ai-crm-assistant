
namespace Features.AI_Assistans.Users.CreateUser;

public static class CreateUserEndpoint
{
    public static IEndpointRouteBuilder MapCreateUserEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (
            CreateUserRequest request,
            AppDbContext context,
            CancellationToken cancellationToken) =>
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
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
        .WithName("CreateUser");

        return app;
    }
}



