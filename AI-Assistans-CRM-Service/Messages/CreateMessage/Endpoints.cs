
using Features.AI_Assistans.Messages.CreateMessage;
using Features.AI_Assistans.Services;


public static class CreateMessageEndpoint
    {
        public static IEndpointRouteBuilder MapCreateMessageEndpoint(
            this IEndpointRouteBuilder app)
        {
            app.MapPost("/messages", async (
                CreateMessageRequest request,
                IAppDbContext context,
                CancellationToken cancellationToken) =>
            {
                var conversationExists = await context.Conversations
                    .AnyAsync(x => x.Id == request.ConversationId,
                        cancellationToken);

                if (!conversationExists)
                {
                    return Results.NotFound(
                        "Conversation not found");
                }

                var message = new Message
                {
                    ConversationId = request.ConversationId,
                    Role = request.Role,
                    Content = request.Content,
                    SentAt = DateTime.UtcNow
                };

                await context.Messages.AddAsync(
                    message,
                    cancellationToken);

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new CreateMessageResponse
                {
                    Id = message.Id,
                    SentAt = message.SentAt
                });
            })
            .RequireAuthorization()
            .WithName("CreateMessage");

            return app;
        }
    }


