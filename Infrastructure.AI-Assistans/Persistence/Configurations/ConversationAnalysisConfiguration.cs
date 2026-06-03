using Domain.AI_Assistans.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.AI_Assistans.Persistence.Configurations
{
    public class ConversationAnalysisConfiguration
        : IEntityTypeConfiguration<ConversationAnalysis>
    {
        public void Configure(EntityTypeBuilder<ConversationAnalysis> builder)
        {
            builder.ToTable("ConversationAnalyses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Summary)
            .HasColumnType("text");

            builder.Property(x => x.SuggestedReply)
                .HasColumnType("text");

            builder.Property(x => x.SuggestedNextAction)
                .HasColumnType("text");

            builder.Property(x => x.Sentiment)
                .HasMaxLength(200);

            builder.Property(x => x.ModelName)
                .HasMaxLength(200);

            builder.Property(x => x.Version)
                .HasMaxLength(50);
        }
    }

}
