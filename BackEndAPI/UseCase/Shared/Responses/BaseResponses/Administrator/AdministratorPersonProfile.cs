using UseCase.Shared.Responses.BaseResponses.User;

namespace UseCase.Shared.Responses.BaseResponses.Administrator
{
    public class AdministratorPersonProfile : UserPersonProfile
    {
        public required Guid PersonId { get; init; }
        public required string? Login { get; init; }
    }
}
