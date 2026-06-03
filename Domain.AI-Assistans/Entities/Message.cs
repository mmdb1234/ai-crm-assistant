
using Domain.AI_Assistans.Entities.baseEn;
using Domain.AI_Assistans.Enums;

namespace Domain.AI_Assistans.Entities
{
    public class Message : BaseEntity 
    { 
        public long Id { get; set; } 
        public Guid ConversationId { get; set; } 
        public Conversation? Conversation { get; set; } 
        public MessageRole Role { get; set; } 
        public string Content { get; set; } = default!; 
        public DateTime SentAt { get; set; } = DateTime.UtcNow; 
    }
}
