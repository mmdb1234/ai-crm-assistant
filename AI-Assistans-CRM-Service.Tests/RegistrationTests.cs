using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Dtos;
using Infrastructure.AI_Assistans.Persistence;

namespace AI_Assistans_CRM_Service.Tests;

public class RegistrationTests
{
    [Fact]
    public async Task CompanyRegisterAsync_ValidRequest_ShouldReturnAuthResponse()
    {
        var repo = new MockCompanyRepository();
        var tokenService = new MockTokenService();
        var sut = new AuthService(tokenService, repo);

        var result = await sut.CompanyRegisterAsync(new RegisterRequestDto
        {
            Name = "New Corp",
            Username = "newuser",
            Password = "securePass123",
            Email = "new@corp.com"
        });

        Assert.NotNull(result);
        Assert.Equal("mock-access-token", result.Token);
        Assert.Equal("mock-refresh-token", result.RefreshToken);
        Assert.Equal("New Corp", result.Name);
        Assert.Equal(1, result.CompanyId);
    }

    [Fact]
    public async Task CompanyRegister_DuplicateUsername_ShouldThrowException()
    {
        var repo = new MockCompanyRepository { UsernameAlreadyExists = true };
        var tokenService = new MockTokenService();
        var sut = new AuthService(tokenService, repo);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            sut.CompanyRegisterAsync(new RegisterRequestDto
            {
                Name = "New Corp",
                Username = "existing",
                Password = "securePass123"
            }));
    }

    private class MockCompanyRepository : ICompanyRepository
    {
        public bool UsernameAlreadyExists { get; set; }
        private int _nextId = 1;

        public Task<Company?> GetByUsernameAsync(string username)
            => Task.FromResult<Company?>(null);

        public Task<Company?> GetCompanyAsync(int companyID)
            => Task.FromResult<Company?>(null);

        public Task<bool> UsernameExistsAsync(string username)
            => Task.FromResult(UsernameAlreadyExists);

        public Task<Company> CreateAsync(Company company)
        {
            company.Id = _nextId++;
            return Task.FromResult(company);
        }

        public Task SaveRefreshTokenAsync(int companyId, string refreshToken)
            => Task.CompletedTask;

        public Task<bool> CanAnalyzeAsync(int companyId)
            => Task.FromResult(true);

        public Task IncrementAnalysisCountAsync(int companyId)
            => Task.CompletedTask;
    }

    private class MockTokenService : ITokenService
    {
        public string GenerateToken(Company entity) => "mock-access-token";
        public string GenerateRefreshToken() => "mock-refresh-token";
        public bool ValidateToken(string token) => true;
        public int? GetCompanyIdFromToken(string token) => 1;
    }
}