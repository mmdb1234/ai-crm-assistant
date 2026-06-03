

namespace Domain.AI_Assistans.AI
{
    public class ConversationAnalysisResult
    {
        public string Summary { get; set; } = default!;

        public string Sentiment { get; set; } = default!;

        public int LeadScore { get; set; }

        public string SuggestedReply { get; set; } = default!;

        public string SuggestedNextAction { get; set; } = default!;
    }

}
