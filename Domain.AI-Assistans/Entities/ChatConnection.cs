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

    [Required, MaxLength(200)]
    public string ExternalChatId { get; set; } = default!;

    [MaxLength(200)]
    public string? ExternalUsername { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(200)]
    public string? WebhookToken { get; set; }

    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

    public Guid? ActiveConversationId { get; set; }
    public Conversation? ActiveConversation { get; set; }
}
