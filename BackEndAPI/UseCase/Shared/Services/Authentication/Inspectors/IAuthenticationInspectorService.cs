// Ignore Spelling: Validator, Validators
using System.Security.Claims;

namespace UseCase.Shared.Services.Authentication.Inspectors
{
    public interface IAuthenticationInspectorService
    {
        bool IsValidJwt(string jwt, bool allowedExpired = false);
        string? GetClaimsName(IEnumerable<Claim> claims);
        string? GetClaimsName(string jwt, bool allowedExpired = false);
    }
}
