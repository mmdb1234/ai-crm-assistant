
using Domain.AI_Assistans.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.AI_Assistans.Factories;

public interface IAIAnalysisServiceFactory
{
    IAIAnalysisService Create(
        string? providerName = null);
}

public class AIAnalysisServiceFactory
    : IAIAnalysisServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    private readonly AIProvidersOptions _options;

    public AIAnalysisServiceFactory(
        IServiceProvider serviceProvider,
        IOptions<AIProvidersOptions> options)
    {
        _serviceProvider = serviceProvider;

        _options = options.Value;
    }

    public IAIAnalysisService Create(
        string? providerName = null)
    {
        var provider =
            (providerName ?? _options.DefaultProvider)
            .ToLower();

        return provider switch
        {
            "openai"
                => _serviceProvider
                    .GetRequiredKeyedService<IAIAnalysisService>(
                        "OpenAI"),

            "deepseek"
                => _serviceProvider
                    .GetRequiredKeyedService<IAIAnalysisService>(
                        "DeepSeek"),

            "gemini"
                => _serviceProvider
                    .GetRequiredKeyedService<IAIAnalysisService>(
                        "Gemini"),

            "openrouter" 
            => _serviceProvider
            .GetRequiredKeyedService<IAIAnalysisService>(
                "OpenRouter"),

            _ => throw new ArgumentException(
                $"Unknown AI provider: {provider}")
        };
    }
}
