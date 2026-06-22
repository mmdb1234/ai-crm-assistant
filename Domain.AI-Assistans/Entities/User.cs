

using Domain.AI_Assistans.Entities.baseEn;
using System.ComponentModel.Design;
using System.Text.Json.Serialization;

namespace Domain.AI_Assistans.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = default!;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        [JsonIgnore]
        public ICollection<Conversation> Conversations { get; set; }
            = new List<Conversation>();
    }


}
