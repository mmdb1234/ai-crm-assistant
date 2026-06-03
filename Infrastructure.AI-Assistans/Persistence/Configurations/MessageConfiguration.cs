using Domain.AI_Assistans.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AI_Assistans.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message> { 
        public void Configure(EntityTypeBuilder<Message> builder) 
        { 
           
            builder.ToTable("Messages"); builder.HasKey(x => x.Id); 
           
            builder.Property(x => x.Content).IsRequired(); 
            
            builder.Property(x => x.Role).HasConversion<int>(); 
            
            builder.HasIndex(x => x.ConversationId); } 
    }
}
