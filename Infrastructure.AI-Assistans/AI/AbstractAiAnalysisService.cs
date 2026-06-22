
using Domain.AI_Assistans.AI;
using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Interfaces;
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

            var response =
                await SendPromptAsync(
                    prompt,
                    cancellationToken);

            Console.WriteLine("=== AI RESPONSE ===");
            Console.WriteLine(response);

            try
            {
                var result =
                    JsonSerializer.Deserialize<
                        ConversationAnalysisResult>(
                        response,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (result is null)
                {
                    throw new Exception(
                        "AI response deserialized to null");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("=== JSON PARSE ERROR ===");
                Console.WriteLine(ex.Message);

                throw new Exception(
                    $"Failed to parse AI response: {response}");
            }
        }
    }
}
