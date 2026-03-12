using Interface.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Interface.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Service.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IClaimService _claimService;

        public TokenService(IConfiguration configuration, IClaimService claimService)
        {
            _configuration = configuration;
            _claimService = claimService;
        }

        public async Task<string> GetJwtToken(ClientDto client, DateTime? expiresIn, string platformToken)
        {
            DateTime tokenEndDateTimeUTC = DateTime.UtcNow.AddHours(2);

            if (client.UserType == "SA")
                tokenEndDateTimeUTC = DateTime.UtcNow.AddYears(1);

            if (expiresIn.HasValue && expiresIn.Value > DateTime.UtcNow)
                tokenEndDateTimeUTC = expiresIn.Value;

            string secretKey = _configuration["SecretKey:JwtSecretKey"];

            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("JWT Secret Key is missing in configuration.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var claims = new List<System.Security.Claims.Claim>
    {
        new System.Security.Claims.Claim("ClientId", client.Id.ToString()),
        new System.Security.Claims.Claim("ClientName", client.Name ?? ""),
        new System.Security.Claims.Claim("TokenType", client.TokenType ?? ""),
        new System.Security.Claims.Claim("Role", client.Role ?? "")
    };

            if (!string.IsNullOrEmpty(platformToken))
            {
                claims.Add(new System.Security.Claims.Claim("PlatformToken", platformToken));
            }


                foreach (var item in client.clientClaimDtos)
                {
                    var claim = await _claimService.GetClaimById(item.ClaimId);

                    if (claim != null)
                    {
                        claims.Add(new System.Security.Claims.Claim("claim", claim.Permission));
                    }
                }
           

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenEndDateTimeUTC,
                SigningCredentials = creds,
                IssuedAt = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }
    }

}
