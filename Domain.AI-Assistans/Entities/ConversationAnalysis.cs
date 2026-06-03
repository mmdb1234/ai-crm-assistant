

using Domain.AI_Assistans.Entities.baseEn;

namespace Domain.AI_Assistans.Entities
{
    public class ConversationAnalysis : BaseEntity
    {
        public long Id { get; set; }

        public Guid ConversationId { get; set; }

        public Conversation? Conversation { get; set; }

        public string Summary { get; set; } = default!;

        public string Sentiment { get; set; } = default!;

        public int LeadScore { get; set; }

        public string SuggestedReply { get; set; } = default!;

        public string SuggestedNextAction { get; set; } = default!;

        public string ModelName { get; set; } = default!;

        public string Version { get; set; } = "v1";
    }

}
