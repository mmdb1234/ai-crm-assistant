

using Domain.AI_Assistans.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AI_Assistans.Persistence.Configurations
{
    public class ConversationConfiguration
        : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Conversations)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Messages)
                .WithOne(x => x.Conversation)
                .HasForeignKey(x => x.ConversationId);

            builder.HasMany(x => x.Analyses)
                .WithOne(x => x.Conversation)
                .HasForeignKey(x => x.ConversationId);

            builder.HasOne(x => x.Company)
                .WithMany(x => x.Conversations)
                .HasForeignKey(x => x.CompanyId);
        }
    }
}