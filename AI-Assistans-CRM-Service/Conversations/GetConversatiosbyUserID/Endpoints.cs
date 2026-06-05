
using Microsoft.AspNetCore.Mvc;

namespace Features.AI_Assistans.Conversations.GetConversatiosbyUserID
{
    public static class GetConversationsByUserIDEndpoint
    {
        public static IEndpointRouteBuilder MapGetConversationsByUserIDEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapGet("users/{userid:guid}/conversations", async (
                [FromRoute] Guid userid,
                AppDbContext context,
                CancellationToken cancellationToken) =>
            {
                var conversations = await context.Conversations
                    .Where(c=>c.UserId == userid)
                    .ToListAsync(cancellationToken);


                return Results.Ok(conversations);
            })
            .WithName("GetUserConversations");

            return app;
        }
    }
}
