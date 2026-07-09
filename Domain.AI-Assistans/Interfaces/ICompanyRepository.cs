using Domain.AI_Assistans.Entities;

namespace Domain.AI_Assistans.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByUsernameAsync(string username);
        Task<Company?> GetCompanyAsync(int companyID);
        Task<bool> UsernameExistsAsync(string username);
        Task<Company> CreateAsync(Company company);
        Task SaveRefreshTokenAsync(int companyId, string refreshToken);
        Task<bool> CanAnalyzeAsync(int companyId);
        Task IncrementAnalysisCountAsync(int companyId);
    }
}
