

using Domain.AI_Assistans.Entities;

namespace Domain.AI_Assistans.Interfaces
{
public static class PromptBuilder
    {
        public static string BuildConversationAnalysisPrompt(
            Conversation conversation)
        {
            var messages = string.Join(
                "\n",
                conversation.Messages.Select(x =>
                    $"{x.Role}: {x.Content}"));

            return
        $"""
        You are an AI CRM assistant.

        Analyze this customer conversation.

        Conversation:
        {messages}

        Return:
        1. Summary
        2. Customer sentiment
        3. Lead score from 0 to 100
        4. Suggested reply
        5. Suggested next action

        Format:
        SUMMARY:
        SENTIMENT:
        LEAD_SCORE:
        SUGGESTED_REPLY:
        NEXT_ACTION:
        """;
        }
    }

}
