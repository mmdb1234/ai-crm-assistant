using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AI_Assistans.Persistence;

public class ChatConnectionRepository : IChatConnectionRepository
{
    private readonly AppDbContext _context;

    public ChatConnectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ChatConnection?> GetByUserAndPlatformAsync(Guid userId, ChatPlatform platform)
    {
        return await _context.ChatConnections
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId
                                   && x.Platform == platform
                                   && x.IsActive);
    }

    public async Task<List<ChatConnection>> GetByUserIdAsync(Guid userId)
    {
        return await _context.ChatConnections
            .Where(x => x.UserId == userId && x.IsActive)
            .ToListAsync();
    }

    public async Task<ChatConnection> CreateAsync(ChatConnection connection)
    {
        _context.ChatConnections.Add(connection);
        await _context.SaveChangesAsync();
        return connection;
    }

    public async Task UpdateBotTokenAsync(long connectionId, string botToken, string botUsername)
    {
        var connection = await _context.ChatConnections.FindAsync(connectionId);
        if (connection is not null)
        {
            connection.BotToken = botToken;
            connection.BotUsername = botUsername;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeactivateAsync(long connectionId)
    {
        var connection = await _context.ChatConnections.FindAsync(connectionId);
        if (connection is not null)
        {
            connection.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasActiveConnectionAsync(Guid userId, ChatPlatform platform)
    {
        return await _context.ChatConnections
            .AnyAsync(x => x.UserId == userId
                        && x.Platform == platform
                        && x.IsActive);
    }

    public async Task<Conversation?> GetActiveConversationBySenderAsync(
        Guid userId, string externalSenderId, ChatPlatform platform)
    {
        return await _context.Conversations
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x => x.UserId == userId
                                   && x.ExternalSenderId == externalSenderId
                                   && x.ExternalPlatform == platform);
    }
}
