using Domain.AI_Assistans.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AI_Assistans.Persistence.Configurations;

public class ChatConnectionConfiguration : IEntityTypeConfiguration<ChatConnection>
{
    public void Configure(EntityTypeBuilder<ChatConnection> builder)
    {
        builder.ToTable("ChatConnections");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExternalChatId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ExternalUsername)
            .HasMaxLength(200);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.WebhookToken)
            .HasMaxLength(200);

        builder.Property(x => x.Platform)
            .HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ActiveConversation)
            .WithMany()
            .HasForeignKey(x => x.ActiveConversationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => new { x.ExternalChatId, x.Platform }).IsUnique();
        builder.HasIndex(x => x.WebhookToken).IsUnique().HasFilter("[WebhookToken] IS NOT NULL");
    }
}
