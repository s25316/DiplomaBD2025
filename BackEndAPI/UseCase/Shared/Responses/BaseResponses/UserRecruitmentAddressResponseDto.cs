namespace UseCase.Shared.Responses.BaseResponses
{
    public class UserRecruitmentAddressResponseDto
    {
        public int CountryId { get; init; }

        public string CountryName { get; init; } = null!;

        public int StateId { get; init; }

        public string StateName { get; init; } = null!;

        public int CityId { get; init; }

        public string CityName { get; init; } = null!;
    }
}
