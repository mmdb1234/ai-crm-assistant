using System.Text.Json;
using Domain.AI_Assistans.AI;

namespace AI_Assistans_CRM_Service.Tests;

public class AnalysisResultDeserializationTests
{
    [Fact]
    public void Deserialize_ValidJson_ShouldReturnResult()
    {
        var json = """
        {
            "summary": "Customer wants to buy",
            "sentiment": "Positive",
            "leadScore": 90,
            "suggestedReply": "Send proposal",
            "suggestedNextAction": "Schedule call"
        }
        """;

        var result = JsonSerializer.Deserialize<ConversationAnalysisResult>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.Equal("Customer wants to buy", result!.Summary);
        Assert.Equal("Positive", result.Sentiment);
        Assert.Equal(90, result.LeadScore);
        Assert.Equal("Send proposal", result.SuggestedReply);
        Assert.Equal("Schedule call", result.SuggestedNextAction);
    }

    [Fact]
    public void Deserialize_MissingFields_ShouldReturnDefaults()
    {
        var json = """{}""";

        var result = JsonSerializer.Deserialize<ConversationAnalysisResult>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.Empty(result!.Summary);
        Assert.Empty(result.Sentiment);
        Assert.Equal(0, result.LeadScore);
    }

    [Fact]
    public void Deserialize_CamelCaseJson_ShouldWork()
    {
        var json = """{"summary":"test","sentiment":"Neutral","leadScore":50,"suggestedReply":"reply","suggestedNextAction":"action"}""";

        var result = JsonSerializer.Deserialize<ConversationAnalysisResult>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(result);
        Assert.Equal("test", result!.Summary);
        Assert.Equal(50, result.LeadScore);
    }
}
