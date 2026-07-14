using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;

namespace Domain.AI_Assistans.Interfaces;

public interface IChatConnectionRepository
{
    Task<ChatConnection?> GetByUserAndPlatformAsync(Guid userId, ChatPlatform platform);
    Task<List<ChatConnection>> GetByUserIdAsync(Guid userId);
    Task<ChatConnection> CreateAsync(ChatConnection connection);
    Task UpdateBotTokenAsync(long connectionId, string botToken, string botUsername);
    Task DeactivateAsync(long connectionId);
    Task<bool> HasActiveConnectionAsync(Guid userId, ChatPlatform platform);
    Task<Conversation?> GetActiveConversationBySenderAsync(Guid userId, string externalSenderId, ChatPlatform platform);
}
