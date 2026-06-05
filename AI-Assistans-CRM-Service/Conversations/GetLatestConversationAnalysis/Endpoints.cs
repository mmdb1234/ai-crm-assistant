

namespace Features.AI_Assistans.Conversations.CreateConversation;

public static class GetLatestConversationAnalysisEndpoint
    {
        public static IEndpointRouteBuilder
            MapGetLatestConversationAnalysisEndpoint(
            this IEndpointRouteBuilder app)
        {
        app.MapGet(
            "/conversations/{conversationId:guid}/analysis/latest",
            async (
                Guid conversationId,
                AppDbContext context,
                CancellationToken cancellationToken) =>
            {
                var analysis = await context.ConversationAnalyses
                    .Where(x => x.ConversationId == conversationId)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync(cancellationToken);

                if (analysis is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(analysis);
            })
        .WithName("GetLatestConversationAnalysis");

            return app;
        }
    }






