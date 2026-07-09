
using Domain.AI_Assistans.Entities;
using Microsoft.EntityFrameworkCore;

namespace Features.AI_Assistans.Services
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Company> Companies { get; }
        DbSet<Conversation> Conversations { get; }
        DbSet<Message> Messages { get; }
        DbSet<ConversationAnalysis> ConversationAnalyses { get; }
        DbSet<ChatConnection> ChatConnections { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
