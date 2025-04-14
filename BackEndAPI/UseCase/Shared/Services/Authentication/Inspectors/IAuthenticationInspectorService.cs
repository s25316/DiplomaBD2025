// Ignore Spelling: Validator, Validators
using Domain.Features.People.ValueObjects.Ids;
using System.Security.Claims;

namespace UseCase.Shared.Services.Authentication.Inspectors
{
    public interface IAuthenticationInspectorService
    {
        bool IsValidJwt(string jwt, bool allowedExpired = false);
        string? GetClaimsName(IEnumerable<Claim> claims);
        string? GetClaimsName(string jwt, bool allowedExpired = false);
        PersonId GetPersonId(IEnumerable<Claim> claims);
    }
}
