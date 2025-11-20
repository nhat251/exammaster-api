using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Common.Exceptions;

namespace Config
{
    public class JwtUtils
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtUtils> _logger;
        private readonly JwtSecurityTokenHandler _handler;

        public JwtUtils(JwtSettings jwtSettings, ILogger<JwtUtils> logger, JwtSecurityTokenHandler handler)
        {
            _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
            _logger = logger;
            _handler = handler;
        }

        public String GenerateAccessToken(String userId, String userName, IList<string> roles)
        {

            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Username cannot be null or empty.", nameof(userName));
            if (roles == null || !roles.Any()) throw new ArgumentException("Roles cannot be null or empty.", nameof(roles));

            if (_jwtSettings.Key.Length < 32)
                throw new InvalidOperationException("JWT Key must be at least 32 characters long.");

            var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public static string ParseAccessTokenFromHeader(string header)
        {
            return header.Substring(7);
        }

        public DateTime GetAccessTokenExpiredTime(string accessToken)
        {
            ValidateFormmatToken(accessToken);

            var jwtToken = _handler.ReadJwtToken(accessToken);

            var expUnix = jwtToken.Payload.Expiration;
            if (expUnix == null)
                throw new InvalidOperationException("Token does not contain 'exp' claim.");

            return DateTimeOffset.FromUnixTimeSeconds((long)expUnix).DateTime;

        }

        /// <summary>
        /// Use this method to get any claim from the access token.
        /// Use <see cref="System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames"/> for standard claims.
        /// </summary>
        /// <param name="accessToken">The JWT access token string.</param>
        /// <param name="claimType">The claim type to retrieve.</param>
        /// <returns>The claim value, or null if not present.</returns>
        public string? GetClaim(string accessToken, string claimType)
        {
            ValidateFormmatToken(accessToken);

            var jwtToken = _handler.ReadJwtToken(accessToken);
            return jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

        }

        private void ValidateFormmatToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new AppException(ErrorCode.UNAUTHORIZED);

            if (!_handler.CanReadToken(accessToken))
                throw new AppException(ErrorCode.UNAUTHORIZED);
        }


    }
}
