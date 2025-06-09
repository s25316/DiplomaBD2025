using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using UseCase.MongoDb;

namespace BackEndAPI.Middlewares
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        // Properties
        private static readonly string _authorizationHeader = "Authorization";
        private static readonly string _bearerString = "Bearer";

        private readonly IMongoDbService _mongoService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        // Constructor
        public ClaimsTransformation(
            IMongoDbService mongoService,
            IHttpContextAccessor httpContextAccessor)
        {
            _mongoService = mongoService;
            _httpContextAccessor = httpContextAccessor;
        }


        // Methods
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var jwt = GetJwt();

            if (string.IsNullOrWhiteSpace(jwt) ||
                principal.Identity == null ||
                !principal.Identity.IsAuthenticated)
            {
                return principal;
            }

            var claims = new List<Claim>(principal.Claims);
            bool claimsWereModified = false;
            var personId = GetPersonId(principal)
                ?? throw new Exception("Something Changed in JWT Generation");

            var userMiddlewareData = await _mongoService.GetUserMiddlewareDataAsync(
                personId,
                jwt,
                CancellationToken.None);

            if (userMiddlewareData.HasRemoved ||
                userMiddlewareData.HasBlocked ||
                userMiddlewareData.HasLogOut)
            {
                // 403 Forbidden
                return new ClaimsPrincipal(new ClaimsIdentity());
            }

            if (userMiddlewareData.IsAdmin ||
                personId == Program.BaseAdministrator)
            {
                claimsWereModified = true;
                AddAdministratorClaim(claims);
            }

            if (claimsWereModified)
            {
                return new ClaimsPrincipal(
                    new ClaimsIdentity(
                        claims,
                        principal.Identity.AuthenticationType));
            }

            return principal;
        }

        // Static Methods
        private static void AddAdministratorClaim(List<Claim> claims)
        {
            if (!claims.Any(c =>
                c.Type == ClaimTypes.Role &&
                c.Value == "Administrator"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            }
        }

        private static Guid? GetPersonId(ClaimsPrincipal principal)
        {
            var personIdString = principal.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(personIdString) ||
                !Guid.TryParse(personIdString, out var personId))
            {
                return null;
            }
            return personId;
        }

        // Non Static Methods
        private string? GetJwt()
        {
            if (_httpContextAccessor.HttpContext == null ||
               !_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(
                   _authorizationHeader,
                   out var authorizationHeader))
            {
                return null;
            }
            var authHeaderValue = authorizationHeader.ToString();
            if (!authHeaderValue.StartsWith(
                _bearerString,
                StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            return authHeaderValue.Substring(_bearerString.Length).Trim();
        }
    }
}
