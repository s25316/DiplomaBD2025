// Ignore Spelling: Jwt
using Domain.Shared.CustomProviders;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace UseCase.Shared.Services.Authentication.Generators
{
    public class AuthenticationGeneratorService : IAuthenticationGeneratorService
    {
        // Properties
        private readonly static int _countPasswordHashIteration = 10_000;
        private readonly static int _countDaysValidRefreshToken = 2;
        private readonly static int _countMinutesValidJwt = 300;
        private static readonly string _jwtIssuer = Configuration.JwtIssuer;
        private static readonly string _jwtAudience = Configuration.JwtAudience;
        private static readonly string _jwtSecret = Configuration.JwtSecret;

        // Methods
        /// <summary>
        /// Generate a 128-bit salt using secure PRNG
        /// </summary>
        /// <returns></returns>
        public string GenerateSalt() => GenerateRandomString(128 / 8);

        /// <summary>
        /// Password base key derivation function [Standard] - Pbkdf2
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string HashPassword(string salt, string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: Convert.FromBase64String(salt),
               prf: KeyDerivationPrf.HMACSHA1,
               iterationCount: _countPasswordHashIteration,
               numBytesRequested: 256 / 8
               ));
        }

        /// <summary>
        /// Password base key derivation function [Standard] - Pbkdf2
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public (string Salt, string HashedPassword) HashPassword(string password)
        {
            var salt = GenerateSalt();
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: Convert.FromBase64String(salt),
               prf: KeyDerivationPrf.HMACSHA1,
               iterationCount: _countPasswordHashIteration,
               numBytesRequested: 256 / 8
               ));
            return (salt, hashedPassword);
        }

        public (string RefreshToken, DateTime ValidTo) GenerateRefreshToken()
        {
            var refresh = GenerateRandomString(1024);
            var validTo = CustomTimeProvider
                .GetDateTimeNow()
                .AddDays(_countDaysValidRefreshToken);
            return (refresh, validTo);
        }

        public IEnumerable<Claim> GenerateClaims(string name, IEnumerable<string> roles)
        {
            var claims = roles
                .ToHashSet()
                .Select(role => new Claim(ClaimTypes.Role, role))
                .ToList();

            claims.AddRange([
                //Protect Before Replay attack
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name,name),
            ]);
            return claims;
        }

        public JwtSecurityToken GenerateJwt(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var signing = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims.ToArray(),
                expires: DateTime.Now.ToLocalTime().AddMinutes(_countMinutesValidJwt),
                signingCredentials: signing
             );
        }

        public (string Jwt, DateTime ValidTo) GenerateJwt(string name, IEnumerable<string> roles)
        {
            var claims = GenerateClaims(name, roles);
            var token = GenerateJwt(claims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var validTo = token.ValidTo.ToLocalTime();
            return (tokenString, validTo);
        }

        // Private Methods 
        private static string GenerateRandomString(int byteSize)
        {
            byte[] array = new byte[byteSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(array);
            }
            return Convert.ToBase64String(array);
        }
    }
}
