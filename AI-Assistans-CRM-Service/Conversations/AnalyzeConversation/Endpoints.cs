using Domain.AI_Assistans.Interfaces;
using Infrastructure.AI_Assistans.Factories;
using Features.AI_Assistans.Services;
using System.Security.Claims;

namespace Features.AI_Assistans.Conversations.AnalyzeConversation;

public static class CreateAnalyzeConversation
{
    public static IEndpointRouteBuilder MapCreateAnalyzeConversationEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/conversations/{conversationId:guid}/analyze",
            async (
                Guid conversationId,
                ClaimsPrincipal company,
                IAIAnalysisServiceFactory aiServiceFactory,
                [FromQuery] string? provider,
                IAppDbContext context,
                ICompanyRepository companyRepo,
                CancellationToken cancellationToken) =>
            {
                var companyIdClaim = company.FindFirst("CompanyId")?.Value;

                if (string.IsNullOrEmpty(companyIdClaim))
                    return Results.Unauthorized();

                var companyId = int.Parse(companyIdClaim);

                var canAnalyze = await companyRepo.CanAnalyzeAsync(companyId);
                if (!canAnalyze)
                {
                    return Results.StatusCode(429);
                }

                var conversation = await context.Conversations
                    .Where(x => x.Id == conversationId)
                    .Include(x => x.Messages)
                    .FirstOrDefaultAsync(cancellationToken);

                if (conversation is null)
                {
                    return Results.NotFound("Conversation not found");
                }

                if (!conversation.Messages.Any())
                {
                    return Results.BadRequest("Conversation has no messages");
                }

                var aiService = aiServiceFactory.Create(provider);

                var analysisResult = await aiService.AnalyzeConversationAsync(
                    conversation,
                    cancellationToken);

                var analysis = new ConversationAnalysis
                {
                    ConversationId = conversationId,
                    LeadScore = analysisResult.LeadScore,
                    ModelName = aiService.ModelName,
                    Sentiment = analysisResult.Sentiment,
                    SuggestedNextAction = analysisResult.SuggestedNextAction,
                    SuggestedReply = analysisResult.SuggestedReply,
                    Summary = analysisResult.Summary,
                    Version = "v1"
                };

                await context.ConversationAnalyses.AddAsync(analysis, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                await companyRepo.IncrementAnalysisCountAsync(companyId);

                return Results.Ok(analysis);
            })
        .RequireAuthorization()
        .WithName("AnalyzeConversation");

        return app;
    }
}
