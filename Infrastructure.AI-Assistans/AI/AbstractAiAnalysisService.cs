
using Domain.AI_Assistans.AI;
using Domain.AI_Assistans.Builders;
using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.AI_Assistans.AI;

public abstract class AbstractAiAnalysisService : IAIAnalysisService
{
    protected readonly HttpClient _httpClient;
    protected readonly AIProviderConfig _config;
    protected readonly ILogger _logger;

    protected AbstractAiAnalysisService(
        HttpClient httpClient,
        AIProviderConfig config,
        ILogger logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public abstract string ModelName { get; }

    protected abstract Task<string> SendPromptAsync(string prompt, CancellationToken cancellationToken);

    public async Task<ConversationAnalysisResult> AnalyzeConversationAsync(
        Conversation conversation,
        CancellationToken cancellationToken = default)
    {
        var prompt = PromptBuilder.BuildConversationAnalysisPrompt(conversation);

        var response = await SendPromptAsync(prompt, cancellationToken);

        _logger.LogDebug("AI Response: {Response}", response);

        try
        {
            var result = JsonSerializer.Deserialize<ConversationAnalysisResult>(
                response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is null)
            {
                throw new InvalidOperationException("AI response deserialized to null");
            }

            return result;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response: {Response}", response);
            throw new InvalidOperationException($"Failed to parse AI response: {response[..Math.Min(response.Length, 200)]}");
        }
    }
}
