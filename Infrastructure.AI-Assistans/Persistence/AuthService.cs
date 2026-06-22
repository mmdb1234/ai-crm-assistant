using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Dtos;
using Features.AI_Assistans.Services;
using System.Data;

namespace Infrastructure.AI_Assistans.Persistence
{
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

            if (company.Password != request.Password) // فعلاً ساده
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

        public async Task<AuthResponseDto> RefreshTokenAsync(
    RefreshTokenDto request)
        {
            var companyid = _tokenService.GetCompanyIdFromToken(request.AccessToken) ?? 0;

            if(companyid == 0)
                throw new UnauthorizedAccessException("Invalid access token");

            var company = await _companyRepository
                .GetCompanyAsync(companyid);

            

            if (company is null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            if (company.RefreshToken != request.RefreshToken)
                throw new UnauthorizedAccessException("Invalid refresh token");

            if (company.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    throw new UnauthorizedAccessException("Refresh token expired");


            var newAccessToken =
                _tokenService.GenerateToken(company);

            var newRefreshToken =
                _tokenService.GenerateRefreshToken();

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
    }
}
