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

        builder.Property(x => x.BotToken)
            .HasMaxLength(500);

        builder.Property(x => x.BotUsername)
            .HasMaxLength(200);

        builder.Property(x => x.PhoneNumberId)
            .HasMaxLength(100);

        builder.Property(x => x.BusinessPhone)
            .HasMaxLength(20);

        builder.Property(x => x.Platform)
            .HasConversion<int>();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId);
    }
}
