namespace Features.AI_Assistans.Services;

public record TelegramBotInfo
{
    public bool Ok { get; init; }
    public TelegramBotUser? Result { get; init; }
}

public record TelegramBotUser
{
    public long Id { get; init; }
    public string? Username { get; init; }
    public string? FirstName { get; init; }
}

public interface ITelegramBotService
{
    Task<TelegramBotInfo?> GetBotInfoAsync(string botToken);
    Task<bool> SetWebhookAsync(string botToken, string webhookUrl);
}
