using System.ComponentModel.DataAnnotations;
using Domain.AI_Assistans.Entities.baseEn;
using Domain.AI_Assistans.Enums;
using System.Text.Json.Serialization;

namespace Domain.AI_Assistans.Entities
{
    public class Conversation : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public Guid UserId { get; set; }

        public User? User { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        [MaxLength(200)]
        public string? ExternalSenderId { get; set; }

        public ChatPlatform? ExternalPlatform { get; set; }

        public ICollection<Message> Messages { get; set; }
            = new List<Message>();
        public ICollection<ConversationAnalysis> Analyses { get; set; }
            = new List<ConversationAnalysis>();
    }
}
