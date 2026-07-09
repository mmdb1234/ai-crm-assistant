using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Dtos;
using Features.AI_Assistans.Services;
using System.Security.Cryptography;

namespace Infrastructure.AI_Assistans.Persistence;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly ICompanyRepository _companyRepository;

    public AuthService(
        ITokenService tokenService,
        ICompanyRepository companyRepository)
    {
        _tokenService = tokenService;
        _companyRepository = companyRepository;
    }

    public async Task<AuthResponseDto> CompanyLoginAsync(LoginRequestDto request)
    {
        var company = await _companyRepository
            .GetByUsernameAsync(request.Username);

        if (company == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        if (!VerifyPassword(request.Password, company.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = _tokenService.GenerateToken(company);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _companyRepository
            .SaveRefreshTokenAsync(company.Id, refreshToken);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            CompanyId = company.Id,
            Name = company.Name
        };
    }

    public async Task<AuthResponseDto> CompanyRegisterAsync(RegisterRequestDto request)
    {
        if (await _companyRepository.UsernameExistsAsync(request.Username))
            throw new InvalidOperationException("Username already exists");

        var company = new Company
        {
            Name = request.Name,
            Username = request.Username,
            Password = HashPassword(request.Password),
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            CompanyRole = CompanyRole.Owner
        };

        await _companyRepository.CreateAsync(company);

        var token = _tokenService.GenerateToken(company);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _companyRepository
            .SaveRefreshTokenAsync(company.Id, refreshToken);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            CompanyId = company.Id,
            Name = company.Name
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto request)
    {
        var companyid = _tokenService.GetCompanyIdFromToken(request.AccessToken) ?? 0;

        if (companyid == 0)
            throw new UnauthorizedAccessException("Invalid access token");

        var company = await _companyRepository
            .GetCompanyAsync(companyid);

        if (company is null)
            throw new UnauthorizedAccessException("Invalid refresh token");

        if (company.RefreshToken != request.RefreshToken)
            throw new UnauthorizedAccessException("Invalid refresh token");

        if (company.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token expired");

        var newAccessToken = _tokenService.GenerateToken(company);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        await _companyRepository.SaveRefreshTokenAsync(
            company.Id,
            newRefreshToken);

        return new AuthResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            CompanyId = company.Id,
            Name = company.Name
        };
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            100_000,
            HashAlgorithmName.SHA256,
            32);

        var result = new byte[48];
        Array.Copy(salt, 0, result, 0, 16);
        Array.Copy(hash, 0, result, 16, 32);

        return Convert.ToBase64String(result);
    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            var bytes = Convert.FromBase64String(hashedPassword);

            if (bytes.Length != 48)
                return password == hashedPassword;

            var salt = new byte[16];
            var hash = new byte[32];
            Array.Copy(bytes, 0, salt, 0, 16);
            Array.Copy(bytes, 16, hash, 0, 32);

            var computedHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                100_000,
                HashAlgorithmName.SHA256,
                32);

            return CryptographicOperations.FixedTimeEquals(hash, computedHash);
        }
        catch
        {
            return password == hashedPassword;
        }
    }
}
