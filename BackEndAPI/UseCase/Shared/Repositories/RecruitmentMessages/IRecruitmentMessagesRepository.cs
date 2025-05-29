using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.Recruitments.ValueObjects.Ids;
using UseCase.Shared.Requests.QueryParameters;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Shared.Repositories.RecruitmentMessages
{
    public interface IRecruitmentMessagesRepository
    {
        Task<ResultMetadataResponse> CreateAsync(
            PersonId personId,
            RecruitmentId recruitmentId,
            string message,
            bool isPerson,
            CancellationToken cancellationToken);

        Task<ItemsResponse<MessageDto>> GetAsync(
            PersonId personId,
            RecruitmentId recruitmentId,
            bool isPerson,
            PaginationQueryParametersDto pagination,
            bool ascending,
            CancellationToken cancellationToken);
    }
}
