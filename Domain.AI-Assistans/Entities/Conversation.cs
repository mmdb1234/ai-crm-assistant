

using Domain.AI_Assistans.Entities.baseEn;

namespace Domain.AI_Assistans.Entities
{
    public class Conversation : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public Guid UserId { get; set; }

        public User? User { get; set; }

        public ICollection<Message> Messages { get; set; }
            = new List<Message>();

        public ICollection<ConversationAnalysis> Analyses { get; set; }
            = new List<ConversationAnalysis>();
    }

}
