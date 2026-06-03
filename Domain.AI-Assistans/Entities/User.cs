

using Domain.AI_Assistans.Entities.baseEn;

namespace Domain.AI_Assistans.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = default!;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public ICollection<Conversation> Conversations { get; set; }
            = new List<Conversation>();
    }


}
