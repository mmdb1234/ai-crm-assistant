
namespace Features.AI_Assistans.Conversations.CreateConversation
{
    public class CreateConversationRequest 
    { 
        public string Title { get; set; } = default!; 
        public string? Description { get; set; } 
        public Guid UserId { get; set; } 
    }
}
