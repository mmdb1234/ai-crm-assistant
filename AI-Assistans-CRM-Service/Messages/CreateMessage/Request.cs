using Domain.AI_Assistans.Enums;

namespace Features.AI_Assistans.Messages.CreateMessage
{
    public class CreateMessageRequest 
    { 
        public Guid ConversationId { get; set; } 
        public MessageRole Role { get; set; } 
        public string Content { get; set; } = default!; 
    }
}
