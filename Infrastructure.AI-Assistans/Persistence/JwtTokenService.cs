

namespace Infrastructure.AI_Assistans.Persistence
{
    using Domain.AI_Assistans.Entities;
    using Domain.AI_Assistans.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    namespace Infrastructure.Services
    {
        public class JwtTokenService : ITokenService
        {
            private readonly IConfiguration _configuration;

            public JwtTokenService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string GenerateToken(Company entity) 
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, entity.Id.ToString()),
                new Claim("CompanyId", entity.Id.ToString()),
                new Claim("Role", entity.CompanyRole.ToString()),
            };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(8),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            public string GenerateRefreshToken()
            {
                return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            }

            public bool ValidateToken(string token)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out _);

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public int? GetCompanyIdFromToken(string token)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var companyIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "CompanyId");
                if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId))
                    return companyId;

                return null;
            }
        }
    }
}
