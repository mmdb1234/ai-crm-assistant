

namespace Domain.AI_Assistans.AI
{
    public class ConversationAnalysisResult
    {
        public string Summary { get; set; } = string.Empty;

        public string Sentiment { get; set; } = string.Empty;

        public int LeadScore { get; set; }

        public string SuggestedReply { get; set; } = string.Empty;

        public string SuggestedNextAction { get; set; } = string.Empty;
    }

}
