

namespace Features.AI_Assistans.Conversations.GetConversation
{
    public static class GetConversationEndpoint
    {
        public static IEndpointRouteBuilder MapGetConversationEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapGet("/conversations/{id:guid}", async (
                Guid id,
                AppDbContext context,
                CancellationToken cancellationToken) =>
            {
                var conversation = await context.Conversations
                    .Include(x => x.Messages)
                    .Include(x => x.Analyses)
                    .FirstOrDefaultAsync(x => x.Id == id,
                        cancellationToken);

                if (conversation is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(conversation);
            })
            .WithName("GetConversation");

            return app;
        }
    }

}
