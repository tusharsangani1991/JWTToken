using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Model;

namespace WebAPI.Utilities.Jwt
{
    public interface IJwtAuthManager
    {
        JwtAuthResult GenerateJwtToken(string authId, string authPayload);
        //(ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
        (JwtSecurityToken, bool) IsTokenValid(string accessToken);
    }
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly JwtTokenConfig _jwtTokenConfig;
        private readonly byte[] _secret;

        public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;
            _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
        }

        public JwtAuthResult GenerateJwtToken(string authId, string authPayload)
        {
            var claims = new List<Claim>() { new Claim(ClaimTypes.PrimarySid, authId), new Claim(ClaimTypes.Hash, authPayload) };

            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
            var jwtToken = new JwtSecurityToken(
                _jwtTokenConfig.Issuer,
                shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
                claims,
                expires: DateTime.Now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);


            var refreshToken = new RefreshToken
            {
                TokenString = GenerateRefreshTokenString(),
                ExpireAt = DateTime.Now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
            };

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    throw new SecurityTokenException("Invalid token");
                }
                var principal = new JwtSecurityTokenHandler()
                    .ValidateToken(token,
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = _jwtTokenConfig.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(_secret),
                            ValidAudience = _jwtTokenConfig.Audience,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        },
                        out var validatedToken);
                return (principal, validatedToken as JwtSecurityToken);
            }
            catch (Exception ex)
            {
                //throw new SecurityTokenException("Invalid token");
                return (null, null as JwtSecurityToken);
            }
        }

        public (JwtSecurityToken, bool) IsTokenValid(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new SecurityTokenException("Invalid token");
            }
            var (principal, jwtToken) = DecodeJwtToken(accessToken);
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                //throw new SecurityTokenException("Invalid token");
                return (jwtToken, false);
            }

            return (jwtToken, true);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[128];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
