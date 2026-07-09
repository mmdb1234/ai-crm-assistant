using Domain.AI_Assistans.Entities;
using System.Text;

namespace Domain.AI_Assistans.Builders;

public static class PromptBuilder
{
    public static string BuildConversationAnalysisPrompt(Conversation conversation)
    {
        var messages = string.Join("\n", conversation.Messages.Select(x => $"{x.Role}: {x.Content}"));

        var sb = new StringBuilder();
        sb.AppendLine("You are an AI CRM assistant.");
        sb.AppendLine();
        sb.AppendLine("Analyze the following customer conversation.");
        sb.AppendLine();
        sb.AppendLine("Conversation:");
        sb.AppendLine(messages);
        sb.AppendLine();
        sb.AppendLine("Return ONLY valid JSON.");
        sb.AppendLine();
        sb.AppendLine("Rules:");
        sb.AppendLine("- leadScore must be integer between 0 and 100");
        sb.AppendLine("- sentiment must be: positive, neutral, or negative");
        sb.AppendLine("- no markdown");
        sb.AppendLine("- no explanation");
        sb.AppendLine("- no extra text");
        sb.AppendLine();
        sb.AppendLine("JSON format:");
        sb.AppendLine("{");
        sb.AppendLine("  \"summary\": \"string\",");
        sb.AppendLine("  \"sentiment\": \"positive\",");
        sb.AppendLine("  \"leadScore\": 85,");
        sb.AppendLine("  \"suggestedReply\": \"string\",");
        sb.AppendLine("  \"suggestedNextAction\": \"string\"");
        sb.AppendLine("}");

        return sb.ToString();
    }
}
