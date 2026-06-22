

using Features.AI_Assistans.Dtos;

namespace Features.AI_Assistans.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> CompanyLoginAsync(LoginRequestDto request);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto request);
    }
}
