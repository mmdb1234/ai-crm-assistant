using Domain.AI_Assistans.Enums;

namespace Features.AI_Assistans.Services;

public record IncomingChatMessage(
    Guid UserId,
    int CompanyId,
    ChatPlatform Platform,
    string ExternalSenderId,
    string? ExternalSenderName,
    string Text,
    string? ExternalMessageId);

public interface IChatIngestionService
{
    void Enqueue(IncomingChatMessage message);
}
