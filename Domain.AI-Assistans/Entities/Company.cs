

using Domain.AI_Assistans.Entities.baseEn;
using Domain.AI_Assistans.Enums;

namespace Domain.AI_Assistans.Entities
{
    public class Company : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public CompanyRole CompanyRole { get; set; }
        public string RefreshToken { get; set; } = "";
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int DailyAnalysisCount { get; set; }
        public DateTime? LastAnalysisDate { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}
