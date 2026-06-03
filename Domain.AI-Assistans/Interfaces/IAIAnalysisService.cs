

using Domain.AI_Assistans.AI;
using Domain.AI_Assistans.Entities;

namespace Domain.AI_Assistans.Interfaces
{
    public interface IAIAnalysisService 
    {
        string ModelName { get; }
        Task<ConversationAnalysisResult> AnalyzeConversationAsync(Conversation conversation, CancellationToken cancellationToken = default);
    }
}
