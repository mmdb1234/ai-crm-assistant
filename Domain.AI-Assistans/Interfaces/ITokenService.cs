
using Domain.AI_Assistans.Entities;

namespace Domain.AI_Assistans.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Company entity);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
        int? GetCompanyIdFromToken(string token);
    }

    public interface ICompanyIdentity
    {
        int Id { get; }
        string Username { get; }
    }
}
