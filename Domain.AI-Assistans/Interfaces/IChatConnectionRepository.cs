using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;

namespace Domain.AI_Assistans.Interfaces;

public interface IChatConnectionRepository
{
    Task<ChatConnection?> GetByExternalIdAsync(string externalChatId, ChatPlatform platform);
    Task<List<ChatConnection>> GetByUserIdAsync(Guid userId);
    Task<ChatConnection?> GetByWebhookTokenAsync(string token);
    Task<ChatConnection> CreateAsync(ChatConnection connection);
    Task UpdateConversationAsync(long connectionId, Guid? conversationId);
    Task DeactivateAsync(long connectionId);
}
