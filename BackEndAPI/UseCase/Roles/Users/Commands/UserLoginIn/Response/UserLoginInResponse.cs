namespace UseCase.Roles.Users.Commands.UserLoginIn.Response
{
    public class UserLoginInResponse
    {
        public bool IsSuccess { get; init; } = false;
        public string? Token { get; init; }
        public DateTime? ValidTo { get; init; }
    }
}
