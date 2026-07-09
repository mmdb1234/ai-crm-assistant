

using Features.AI_Assistans.Services;

namespace Features.AI_Assistans.Conversations.GetLatestConversationAnalysis;

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
                IAppDbContext context,
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
        .RequireAuthorization()
        .WithName("GetLatestConversationAnalysis");

            return app;
        }
    }






