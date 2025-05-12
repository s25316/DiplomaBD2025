// Ignore Spelling: Metadata

using MediatR;

namespace UseCase.Shared.Requests
{
    public class BaseRequest<TResponse> : IRequest<TResponse>
    {
        public required RequestMetadata Metadata { get; init; } = null!;
    }
}