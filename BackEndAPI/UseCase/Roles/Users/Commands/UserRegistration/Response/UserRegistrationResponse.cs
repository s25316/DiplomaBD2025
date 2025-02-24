namespace UseCase.Roles.Users.Commands.UserRegistration.Response
{
    public class UserRegistrationResponse
    {
        public required bool IsCreated { get; init; }
        public required string Message { get; init; } = null!;
    }
}
