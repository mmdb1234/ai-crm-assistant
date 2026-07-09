using Domain.AI_Assistans.Entities;
using Features.AI_Assistans.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AI_Assistans.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<ConversationAnalysis> ConversationAnalyses => Set<ConversationAnalysis>();
        public DbSet<ChatConnection> ChatConnections => Set<ChatConnection>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }

}
