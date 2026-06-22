using Domain.AI_Assistans.Entities;

namespace Domain.AI_Assistans.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByUsernameAsync(string username);
        Task<Company?> GetCompanyAsync(int companyID);

        Task SaveRefreshTokenAsync(int companyId, string refreshToken);
    }
}
