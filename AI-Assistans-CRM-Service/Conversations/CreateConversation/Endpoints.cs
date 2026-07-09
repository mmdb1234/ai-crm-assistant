

using Features.AI_Assistans.Services;
using System.Security.Claims;

namespace Features.AI_Assistans.Conversations.CreateConversation;

public static class CreateConversationEndpoint
    {
        public static IEndpointRouteBuilder MapCreateConversationEndpoint(
            this IEndpointRouteBuilder app)
        {
        app.MapPost("/conversations", async (
            ClaimsPrincipal company,
            CreateConversationRequest request,
            IAppDbContext context,
            CancellationToken cancellationToken) =>
        {

            var companyIdClaim = company.FindFirst("CompanyId")?.Value;

            if (string.IsNullOrEmpty(companyIdClaim))
                return Results.Unauthorized();

            var companyId = int.Parse(companyIdClaim);

            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                UserId = request.UserId,
                CompanyId = companyId
            };

            await context.Conversations.AddAsync(
                conversation,
                cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Results.Ok(new CreateConversationResponse
            {
                Id = conversation.Id,
                Title = conversation.Title
            });
        })
        .RequireAuthorization()
        .WithName("CreateConversation");

            return app;
        }
    }




