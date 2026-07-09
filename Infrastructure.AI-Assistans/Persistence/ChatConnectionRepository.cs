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

    public async Task<ChatConnection?> GetByExternalIdAsync(string externalChatId, ChatPlatform platform)
    {
        return await _context.ChatConnections
            .Include(x => x.User)
            .Include(x => x.ActiveConversation)
            .FirstOrDefaultAsync(x => x.ExternalChatId == externalChatId
                                   && x.Platform == platform
                                   && x.IsActive);
    }

    public async Task<List<ChatConnection>> GetByUserIdAsync(Guid userId)
    {
        return await _context.ChatConnections
            .Where(x => x.UserId == userId && x.IsActive)
            .ToListAsync();
    }

    public async Task<ChatConnection?> GetByWebhookTokenAsync(string token)
    {
        return await _context.ChatConnections
            .Include(x => x.User)
            .Include(x => x.ActiveConversation)
            .FirstOrDefaultAsync(x => x.WebhookToken == token && x.IsActive);
    }

    public async Task<ChatConnection> CreateAsync(ChatConnection connection)
    {
        _context.ChatConnections.Add(connection);
        await _context.SaveChangesAsync();
        return connection;
    }

    public async Task UpdateConversationAsync(long connectionId, Guid? conversationId)
    {
        var connection = await _context.ChatConnections.FindAsync(connectionId);
        if (connection is not null)
        {
            connection.ActiveConversationId = conversationId;
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
}
