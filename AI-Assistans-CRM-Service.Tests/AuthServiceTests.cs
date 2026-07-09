using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Dtos;
using Infrastructure.AI_Assistans.Persistence;

namespace AI_Assistans_CRM_Service.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task CompanyLoginAsync_ValidCredentials_ShouldReturnAuthResponse()
    {
        var company = new Company
        {
            Id = 1,
            Name = "Test Corp",
            Username = "test",
            Password = "pass123",
            CompanyRole = CompanyRole.Owner
        };

        var repo = new MockCompanyRepository(company);
        var tokenService = new MockTokenService();
        var sut = new AuthService(tokenService, repo);

        var result = await sut.CompanyLoginAsync(new LoginRequestDto
        {
            Username = "test",
            Password = "pass123"
        });

        Assert.NotNull(result);
        Assert.Equal("mock-access-token", result.Token);
        Assert.Equal("mock-refresh-token", result.RefreshToken);
        Assert.Equal(1, result.CompanyId);
    }

    [Fact]
    public async Task CompanyLoginAsync_WrongPassword_ShouldThrowUnauthorized()
    {
        var company = new Company
        {
            Id = 1,
            Username = "test",
            Password = "correct-pass",
            CompanyRole = CompanyRole.Owner
        };

        var repo = new MockCompanyRepository(company);
        var tokenService = new MockTokenService();
        var sut = new AuthService(tokenService, repo);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            sut.CompanyLoginAsync(new LoginRequestDto
            {
                Username = "test",
                Password = "wrong-pass"
            }));
    }

    [Fact]
    public async Task CompanyLoginAsync_UnknownUser_ShouldThrowUnauthorized()
    {
        var repo = new MockCompanyRepository(null);
        var tokenService = new MockTokenService();
        var sut = new AuthService(tokenService, repo);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            sut.CompanyLoginAsync(new LoginRequestDto
            {
                Username = "unknown",
                Password = "pass"
            }));
    }

    [Fact]
    public async Task RefreshTokenAsync_ExpiredRefreshToken_ShouldThrowUnauthorized()
    {
        var company = new Company
        {
            Id = 1,
            RefreshToken = "old-refresh",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1)
        };

        var repo = new MockCompanyRepository(company);
        var tokenService = new MockTokenService();
        var sut = new AuthService(tokenService, repo);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            sut.RefreshTokenAsync(new RefreshTokenDto
            {
                AccessToken = "token",
                RefreshToken = "old-refresh"
            }));
    }

    private class MockCompanyRepository : ICompanyRepository
    {
        private readonly Company? _company;

        public MockCompanyRepository(Company? company) => _company = company;

        public Task<Company?> GetByUsernameAsync(string username)
            => Task.FromResult(_company);

        public Task<Company?> GetCompanyAsync(int companyID)
            => Task.FromResult(_company);

        public Task<bool> UsernameExistsAsync(string username)
            => Task.FromResult(false);

        public Task<Company> CreateAsync(Company company)
            => Task.FromResult(company);

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
