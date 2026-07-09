using Domain.AI_Assistans.AI;
using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Interfaces;
using Infrastructure.AI_Assistans.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AI_Assistans_CRM_Service.Tests;

public class AIAnalysisServiceFactoryTests
{
    [Fact]
    public void Create_DefaultProvider_ShouldReturnService()
    {
        var options = Options.Create(new AIProvidersOptions { DefaultProvider = "DeepSeek" });

        var services = new ServiceCollection();
        services.AddKeyedScoped<IAIAnalysisService, MockAnalysisService>("DeepSeek");
        var serviceProvider = services.BuildServiceProvider();

        var factory = new AIAnalysisServiceFactory(serviceProvider, options);
        var service = factory.Create();

        Assert.NotNull(service);
    }

    [Fact]
    public void Create_SpecifiedProvider_ShouldReturnCorrectService()
    {
        var options = Options.Create(new AIProvidersOptions { DefaultProvider = "DeepSeek" });

        var services = new ServiceCollection();
        services.AddKeyedScoped<IAIAnalysisService, MockAnalysisService>("OpenAI");
        var serviceProvider = services.BuildServiceProvider();

        var factory = new AIAnalysisServiceFactory(serviceProvider, options);
        var service = factory.Create("OpenAI");

        Assert.NotNull(service);
    }

    [Fact]
    public void Create_UnknownProvider_ShouldThrowArgumentException()
    {
        var options = Options.Create(new AIProvidersOptions { DefaultProvider = "DeepSeek" });

        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        var factory = new AIAnalysisServiceFactory(serviceProvider, options);

        Assert.Throws<ArgumentException>(() => factory.Create("UnknownProvider"));
    }

    private class MockAnalysisService : IAIAnalysisService
    {
        public string ModelName => "mock";
        public Task<ConversationAnalysisResult> AnalyzeConversationAsync(
            Conversation conversation,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ConversationAnalysisResult
            {
                Summary = "Mock",
                Sentiment = "Neutral",
                LeadScore = 50,
                SuggestedReply = "Mock reply",
                SuggestedNextAction = "Mock action"
            });
        }
    }
}
