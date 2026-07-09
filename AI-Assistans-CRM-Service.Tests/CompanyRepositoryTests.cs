using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;

namespace AI_Assistans_CRM_Service.Tests;

public class CompanyRepositoryTests
{
    [Fact]
    public void CompanyEntity_ShouldSetPropertiesCorrectly()
    {
        var company = new Company
        {
            Id = 1,
            Name = "Test Corp",
            Username = "testuser",
            Password = "securepass",
            CompanyRole = CompanyRole.Owner,
            PhoneNumber = "+1234567890",
            Email = "test@corp.com"
        };

        Assert.Equal(1, company.Id);
        Assert.Equal("Test Corp", company.Name);
        Assert.Equal("testuser", company.Username);
        Assert.Equal("securepass", company.Password);
        Assert.Equal(CompanyRole.Owner, company.CompanyRole);
        Assert.Equal("+1234567890", company.PhoneNumber);
        Assert.Equal("test@corp.com", company.Email);
    }

    [Fact]
    public void CompanyEntity_DefaultRefreshToken_ShouldBeEmpty()
    {
        var company = new Company();

        Assert.Equal("", company.RefreshToken);
        Assert.Null(company.RefreshTokenExpiryTime);
    }

    [Fact]
    public void CompanyEntity_ShouldInitializeCollections()
    {
        var company = new Company();

        Assert.NotNull(company.Users);
        Assert.NotNull(company.Conversations);
    }

    [Fact]
    public async Task MockCompanyRepository_GetByUsernameAsync_ShouldReturnCompany()
    {
        var repo = new MockCompanyRepository(GetSampleCompany());

        var result = await repo.GetByUsernameAsync("testuser");

        Assert.NotNull(result);
        Assert.Equal("Test Corp", result!.Name);
    }

    [Fact]
    public async Task MockCompanyRepository_GetByUsernameAsync_UnknownUser_ShouldReturnNull()
    {
        var repo = new MockCompanyRepository(GetSampleCompany());

        var result = await repo.GetByUsernameAsync("unknown");

        Assert.Null(result);
    }

    [Fact]
    public async Task MockCompanyRepository_GetCompanyAsync_ExistingId_ShouldReturnCompany()
    {
        var repo = new MockCompanyRepository(GetSampleCompany());

        var result = await repo.GetCompanyAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test Corp", result!.Name);
    }

    [Fact]
    public async Task MockCompanyRepository_GetCompanyAsync_UnknownId_ShouldReturnNull()
    {
        var repo = new MockCompanyRepository(GetSampleCompany());

        var result = await repo.GetCompanyAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task MockCompanyRepository_SaveRefreshTokenAsync_ShouldUpdate()
    {
        var company = GetSampleCompany();
        var repo = new MockCompanyRepository(company);

        await repo.SaveRefreshTokenAsync(1, "new-refresh-token");

        Assert.Equal("new-refresh-token", company.RefreshToken);
        Assert.True(company.RefreshTokenExpiryTime > DateTime.UtcNow);
    }

    private static Company GetSampleCompany()
    {
        return new Company
        {
            Id = 1,
            Name = "Test Corp",
            Username = "testuser",
            Password = "pass",
            CompanyRole = CompanyRole.Owner,
            Users = new List<User>(),
            Conversations = new List<Conversation>()
        };
    }

    private class MockCompanyRepository : ICompanyRepository
    {
        private readonly Company _company;

        public MockCompanyRepository(Company company) => _company = company;

        public Task<Company?> GetByUsernameAsync(string username)
            => Task.FromResult(_company.Username == username ? _company : null);

        public Task<Company?> GetCompanyAsync(int companyID)
            => Task.FromResult(_company.Id == companyID ? _company : null);

        public Task<bool> UsernameExistsAsync(string username)
            => Task.FromResult(false);

        public Task<Company> CreateAsync(Company company)
            => Task.FromResult(company);

        public Task SaveRefreshTokenAsync(int companyId, string refreshToken)
        {
            if (_company.Id == companyId)
            {
                _company.RefreshToken = refreshToken;
                _company.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }
            return Task.CompletedTask;
        }

        public Task<bool> CanAnalyzeAsync(int companyId)
            => Task.FromResult(true);

        public Task IncrementAnalysisCountAsync(int companyId)
            => Task.CompletedTask;
    }
}
