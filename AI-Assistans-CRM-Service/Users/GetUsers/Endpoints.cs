

using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Features.AI_Assistans.Users.GetUsers
{
    public static class GetUserEndpoint
    {
        public static IEndpointRouteBuilder MapGetUserEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapGet("/users", async (
                [AsParameters] GetRequest request,
                AppDbContext context,
                CancellationToken cancellationToken) =>
            {
                var query = context.Users.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    query = query.Where(u =>
                        u.Username!.Contains(request.SearchText) ||
                        u.PhoneNumber!.Contains(request.SearchText) ||
                        u.Email!.Contains(request.SearchText));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var users = await query
                    .Skip(request.PageIndex * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                return Results.Ok(new GetUserResponse
                {
                    TotalCount = totalCount,

                    Users = users.Select(u => new UserDto
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber
                    }).ToList()
                });
            })
            .WithName("GetUsers");

            return app;
        }
    }

}
