using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UseCase.Shared.Services.Authentication.Generators
{
    public interface IAuthenticationGeneratorService
    {
        string GenerateSalt();
        string HashPassword(string salt, string password);
        (string Salt, string HashedPassword) HashPassword(string password);
        (string RefreshToken, DateTime ValidTo) GenerateRefreshToken();
        IEnumerable<Claim> GenerateClaims(string name, IEnumerable<string> roles);
        JwtSecurityToken GenerateJwt(IEnumerable<Claim> claims);
        (string Jwt, DateTime ValidTo) GenerateJwt(string name, IEnumerable<string> roles);
    }
}
