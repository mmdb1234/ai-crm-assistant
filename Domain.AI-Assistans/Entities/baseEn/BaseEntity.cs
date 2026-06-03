

namespace Domain.AI_Assistans.Entities.baseEn
{
    public abstract class BaseEntity { 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public DateTime? ModifiedAt { get; set; } 
    }
}
