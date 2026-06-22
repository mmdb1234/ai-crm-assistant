

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Features.AI_Assistans.Users.GetUsers
{
    public static class GetUserEndpoint
    {
        public static IEndpointRouteBuilder MapGetUserEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapGet("/users", async (
                ClaimsPrincipal company,
                [AsParameters] GetRequest request,
                IAppDbContext context,
                CancellationToken cancellationToken) =>
            {

                var companyIdClaim = company.FindFirst("CompanyId")?.Value;

                if (string.IsNullOrEmpty(companyIdClaim))
                    return Results.Unauthorized();

                var companyId = int.Parse(companyIdClaim);

                var query = context.Users.Where(u=>u.CompanyId == companyId).AsQueryable();

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
            .RequireAuthorization()
            .WithName("GetUsers");

            return app;
        }
    }

}
