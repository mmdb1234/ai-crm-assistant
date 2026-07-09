using Domain.AI_Assistans.Enums;

namespace Features.AI_Assistans.Services;

public record IncomingChatMessage(
    ChatPlatform Platform,
    string ExternalChatId,
    string? ExternalUsername,
    string Text,
    string? ExternalMessageId);

public interface IChatIngestionService
{
    void Enqueue(IncomingChatMessage message);
}
