

using Domain.AI_Assistans.AI;
using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Interfaces;
using System.Text;
using System.Text.Json;

namespace Infrastructure.AI_Assistans.AI
{
public abstract class AbstractAiAnalysisService
    : IAIAnalysisService
    {
        protected readonly HttpClient _httpClient;
        protected readonly AIProviderConfig _config;

        protected AbstractAiAnalysisService(
            HttpClient httpClient,
            AIProviderConfig config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public abstract string ModelName { get; }

        protected abstract Task<string> SendPromptAsync(
            string prompt,
            CancellationToken cancellationToken);

        public async Task<ConversationAnalysisResult>
            AnalyzeConversationAsync(
                Conversation conversation,
                CancellationToken cancellationToken = default)
        {
            var prompt =
                PromptBuilder.BuildConversationAnalysisPrompt(
                    conversation);

            var aiText = await SendPromptAsync(
                prompt,
                cancellationToken);

            return ParseAnalysis(aiText);
        }

        protected static ConversationAnalysisResult ParseAnalysis(
            string text)
        {
            return new ConversationAnalysisResult
            {
                Summary = Extract(text, "SUMMARY:"),
                Sentiment = Extract(text, "SENTIMENT:"),
                SuggestedReply = Extract(text, "SUGGESTED_REPLY:"),
                SuggestedNextAction = Extract(text, "NEXT_ACTION:"),
                LeadScore = int.TryParse(
                    Extract(text, "LEAD_SCORE:"),
                    out var score)
                        ? score
                        : 0
            };
        }

        protected static string Extract(
            string text,
            string key)
        {
            var lines = text.Split('\n');

            var line = lines.FirstOrDefault(x =>
                x.StartsWith(key));

            return line?
                .Replace(key, "")
                .Trim()
                ?? "";
        }
    }

}
