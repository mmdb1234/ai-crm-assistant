using Domain.AI_Assistans.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.AI_Assistans.AI;

public class OpenRouterAnalysisService : AbstractAiAnalysisService
{
    public override string ModelName => _config.Model;

    public OpenRouterAnalysisService(
        HttpClient httpClient,
        IOptions<AIProvidersOptions> options,
        ILogger<OpenRouterAnalysisService> logger)
        : base(httpClient, options.Value.OpenRouter, logger)
    {
    }

    protected override async Task<string> SendPromptAsync(string prompt, CancellationToken cancellationToken)
    {
        var requestBody = new
        {
            model = _config.Model,
            messages = new[] { new { role = "user", content = prompt } },
            temperature = 0.3
        };

        var requestJson = JsonSerializer.Serialize(requestBody);
        var request = new HttpRequestMessage(HttpMethod.Post, _config.BaseUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config.ApiKey);
        request.Headers.Add( "HTTP-Referer", "https://ai-crm-assistant-ui-production.up.railway.app");

        request.Headers.Add("X-Title","AI Assistant CRM");
        request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        _logger.LogDebug("OpenRouter response: {Response}", responseContent);

        response.EnsureSuccessStatusCode();

        using var document = JsonDocument.Parse(responseContent);
        return document.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}

