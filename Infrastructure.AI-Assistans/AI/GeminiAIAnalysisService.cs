using Domain.AI_Assistans.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Infrastructure.AI_Assistans.AI
{
    public class GeminiAnalysisService : AbstractAiAnalysisService
    {
        public override string ModelName => _config.Model;
        public GeminiAnalysisService(HttpClient httpClient,
            IOptions<AIProvidersOptions> options) : base(httpClient, options.Value.Gemini) { }
        protected override async Task<string> SendPromptAsync(string prompt, CancellationToken cancellationToken)
        {
            var requestBody = new
            {
                contents = new[] {
                    new { parts = new[]
                    {
                        new { text = prompt }
                    }

                 }
                }
            };
            var requestJson = JsonSerializer.Serialize(requestBody);

            var endpoint = $"{_config.BaseUrl}/{_config.Model}:generateContent";

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Add("x-goog-api-key", $"{_config.ApiKey}");

            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            

            var response = await _httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            using var document = JsonDocument.Parse(responseContent);

            return document.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString()!;
        }
    }
}
