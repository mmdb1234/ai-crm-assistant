using Domain.AI_Assistans.Entities.baseEn;
using Domain.AI_Assistans.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.AI_Assistans.Entities;

public class ChatConnection : BaseEntity
{
    public long Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public ChatPlatform Platform { get; set; }

    // Telegram: bot token (encrypted at rest)
    [MaxLength(500)]
    public string? BotToken { get; set; }

    // Telegram: bot username from @BotFather
    [MaxLength(200)]
    public string? BotUsername { get; set; }

    // WhatsApp Business: phone number ID
    [MaxLength(100)]
    public string? PhoneNumberId { get; set; }

    // WhatsApp: display phone number
    [MaxLength(20)]
    public string? BusinessPhone { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
}
