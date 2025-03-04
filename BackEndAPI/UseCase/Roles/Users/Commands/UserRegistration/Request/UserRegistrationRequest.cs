using System.ComponentModel.DataAnnotations;
using UseCase.Roles.Users.Commands.UserRegistration.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.UserRegistration.Request
{
    public class UserRegistrationRequest : RequestTemplate<UserRegistrationResponse>
    {
        [Required]
        public required UserRegistrationCommand Command { get; init; } = null!;
    }
}
