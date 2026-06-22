
using System.Security.Claims;

namespace Features.AI_Assistans.Conversations.GetConversatiosbyUserID
{
    public static class GetConversationsByUserIDEndpoint
    {
        public static IEndpointRouteBuilder MapGetConversationsByUserIDEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapGet("users/{userid:guid}/conversations", async (
                ClaimsPrincipal company,
                [FromRoute] Guid userid,
                IAppDbContext context,
                CancellationToken cancellationToken) =>
            {
                var companyIdClaim = company.FindFirst("CompanyId")?.Value;

                if (string.IsNullOrEmpty(companyIdClaim))
                    return Results.Unauthorized();

                var companyId = int.Parse(companyIdClaim);

                var conversations = await context.Conversations
                    .Where(c=>c.UserId == userid && c.CompanyId == companyId)
                    .ToListAsync(cancellationToken);


                return Results.Ok(conversations);
            })
            .RequireAuthorization()
            .WithName("GetUserConversations");

            return app;
        }
    }
}
