

namespace Features.AI_Assistans.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
