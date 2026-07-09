using Domain.AI_Assistans.Builders;
using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;

namespace AI_Assistans_CRM_Service.Tests;

public class PromptBuilderTests
{
    [Fact]
    public void BuildConversationAnalysisPrompt_ShouldIncludeMessages()
    {
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Messages = new List<Message>
            {
                new() { Role = MessageRole.Customer, Content = "Hello" },
                new() { Role = MessageRole.Support, Content = "Hi there" }
            }
        };

        var prompt = PromptBuilder.BuildConversationAnalysisPrompt(conversation);

        Assert.Contains("Customer: Hello", prompt);
        Assert.Contains("Support: Hi there", prompt);
    }

    [Fact]
    public void BuildConversationAnalysisPrompt_ShouldContainJsonFormat()
    {
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Messages = new List<Message>
            {
                new() { Role = MessageRole.Customer, Content = "Test" }
            }
        };

        var prompt = PromptBuilder.BuildConversationAnalysisPrompt(conversation);

        Assert.Contains("leadScore", prompt);
        Assert.Contains("summary", prompt);
        Assert.Contains("sentiment", prompt);
        Assert.Contains("suggestedReply", prompt);
        Assert.Contains("suggestedNextAction", prompt);
    }

    [Fact]
    public void BuildConversationAnalysisPrompt_EmptyMessages_ShouldStillReturnValidPrompt()
    {
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Title = "Empty",
            Messages = new List<Message>()
        };

        var prompt = PromptBuilder.BuildConversationAnalysisPrompt(conversation);

        Assert.Contains("Conversation:", prompt);
        Assert.Contains("JSON format", prompt);
    }
}
