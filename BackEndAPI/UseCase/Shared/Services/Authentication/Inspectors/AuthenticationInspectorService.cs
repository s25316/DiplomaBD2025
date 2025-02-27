// Ignore Spelling: jwt
using Domain.Features.People.Exceptions;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Templates.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UseCase.Shared.Services.Authentication.Inspectors
{
    public class AuthenticationInspectorService : IAuthenticationInspectorService
    {
        // Properties
        private static readonly string _jwtIssuer = Configuration.JwtIssuer;
        private static readonly string _jwtAudience = Configuration.JwtAudience;
        private static readonly string _jwtSecret = Configuration.JwtSecret;


        // Public Methods
        public bool IsValidJwt(string jwt, bool allowedExpired = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var validationParamiters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = !allowedExpired,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _jwtIssuer,
                ValidAudience = _jwtAudience,
                IssuerSigningKey = secretKey,
            };

            try
            {
                tokenHandler.ValidateToken(jwt, validationParamiters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string? GetClaimsName(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                if (claim.Type == ClaimTypes.Name)
                {
                    return claim.Value;
                }
            }
            return null;
        }

        public string? GetClaimsName(string jwt, bool allowedExpired = false)
        {
            if (!IsValidJwt(jwt, allowedExpired))
            {
                return null;
            }
            var claims = GetClaimsFromJwt(jwt);
            return GetClaimsName(claims);
        }

        public PersonId GetPersonId(IEnumerable<Claim> claims)
        {
            var claimsName = GetClaimsName(claims);
            if (
                string.IsNullOrWhiteSpace(claimsName) ||
                !Guid.TryParse(claimsName, out var id)
                )
            {
                throw new PersonException("", HttpCodeEnum.Unauthorized);
            }
            return new PersonId(id);
        }

        // Private Methods 
        private static IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(jwt);
            var claims = jwtToken.Claims.ToList();
            return claims;
        }
    }
}
