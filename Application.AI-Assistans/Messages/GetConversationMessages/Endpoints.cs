

namespace Features.AI_Assistans.Messages.GetConversationMessages
{
    public static class GetConversationMessagesEndpoint
    {
        public static IEndpointRouteBuilder
            MapGetConversationMessagesEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/conversations/{conversationId:guid}/messages",
                async (
                    Guid conversationId,
                    AppDbContext context,
                    CancellationToken cancellationToken) =>
                {
                    var messages = await context.Messages
                        .Where(x => x.ConversationId == conversationId)
                        .OrderBy(x => x.SentAt)
                        .ToListAsync(cancellationToken);

                    return Results.Ok(messages);
                })
            .WithName("GetConversationMessages");

            return app;
        }
    }


}
