using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;
using Infrastructure.AI_Assistans.Persistence;
using Microsoft.Extensions.Configuration;

namespace AI_Assistans_CRM_Service.Tests;

public class JwtTokenServiceTests
{
    private readonly JwtTokenService _sut;
    private readonly Company _testCompany;

    public JwtTokenServiceTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "This-Is-A-Very-Long-Secret-Key-For-Testing-12345678!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            })
            .Build();

        _sut = new JwtTokenService(config);

        _testCompany = new Company
        {
            Id = 1,
            Name = "Test Corp",
            Username = "test",
            CompanyRole = CompanyRole.Owner
        };
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwt()
    {
        var token = _sut.GenerateToken(_testCompany);

        Assert.False(string.IsNullOrEmpty(token));
        Assert.True(token.Split('.').Length == 3);
    }

    [Fact]
    public void GenerateToken_DifferentCompanies_ShouldReturnDifferentTokens()
    {
        var company2 = new Company { Id = 2, Name = "Other", Username = "other", CompanyRole = CompanyRole.Manager };

        var token1 = _sut.GenerateToken(_testCompany);
        var token2 = _sut.GenerateToken(company2);

        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void ValidateToken_ValidToken_ShouldReturnTrue()
    {
        var token = _sut.GenerateToken(_testCompany);

        var result = _sut.ValidateToken(token);

        Assert.True(result);
    }

    [Fact]
    public void ValidateToken_InvalidToken_ShouldReturnFalse()
    {
        var result = _sut.ValidateToken("invalid-token-here");

        Assert.False(result);
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnBase64String()
    {
        var refreshToken = _sut.GenerateRefreshToken();

        Assert.False(string.IsNullOrEmpty(refreshToken));
        Assert.True(refreshToken.Length > 20);
    }

    [Fact]
    public void GetCompanyIdFromToken_ValidToken_ShouldReturnCompanyId()
    {
        var token = _sut.GenerateToken(_testCompany);

        var companyId = _sut.GetCompanyIdFromToken(token);

        Assert.Equal(1, companyId);
    }

    [Fact]
    public void GetCompanyIdFromToken_InvalidToken_ShouldReturnNull()
    {
        var companyId = _sut.GetCompanyIdFromToken("invalid");

        Assert.Null(companyId);
    }
}
