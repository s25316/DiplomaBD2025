// Ignore Spelling: Metadata

using MediatR;

namespace UseCase.Shared.Templates.Requests
{
    public class BaseRequest<TResponse> : IRequest<TResponse>
    {
        public required RequestMetadata Metadata { get; init; } = null!;
    }
}