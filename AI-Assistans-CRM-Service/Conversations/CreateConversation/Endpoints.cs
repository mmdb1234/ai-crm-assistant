

using Features.AI_Assistans.Services;

namespace Features.AI_Assistans.Conversations.CreateConversation;

public static class CreateConversationEndpoint
    {
        public static IEndpointRouteBuilder MapCreateConversationEndpoint(
            this IEndpointRouteBuilder app)
        {
        app.MapPost("/conversations", async (
            CreateConversationRequest request,
            IAppDbContext context,
            CancellationToken cancellationToken) =>
        {
            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                UserId = request.UserId
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




