

using Domain.AI_Assistans.Entities.baseEn;
using Domain.AI_Assistans.Enums;

namespace Domain.AI_Assistans.Entities
{
    public class Company : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public CompanyRole CompanyRole { get; set; }

        public string RefreshToken { get; set; } = "";
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Conversation> Conversations { get; set; }
    }
}
