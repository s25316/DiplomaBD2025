using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileRemove.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileRemove
{
    public class UserProfileRemoveHandler : IRequestHandler<UserProfileRemoveRequest, ProfileCommandResponse>
    {
        // Properties
        private readonly IMediator _mediator;
        private readonly IPersonRepository _personRepository;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public UserProfileRemoveHandler(
            IMediator mediator,
            IPersonRepository personRepository,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mediator = mediator;
            _personRepository = personRepository;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ProfileCommandResponse> Handle(UserProfileRemoveRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _personRepository.GetAsync(personId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return InValidResponse();
            }
            var domainPreson = selectResult.Item;


            domainPreson.Remove();
            foreach (var @event in domainPreson.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }

            var updateResult = await _personRepository.UpdateAsync(domainPreson, cancellationToken);
            if (updateResult.Code != HttpCode.Ok)
            {
                throw new UseCaseLayerException(updateResult.Metadata.Message);
            }

            return ValidResponse();
        }

        // Static Methods
        public static ProfileCommandResponse ValidResponse()
        {
            return ProfileCommandResponse.PrepareResponse(HttpCode.Ok);
        }

        public static ProfileCommandResponse InValidResponse()
        {
            return ProfileCommandResponse.PrepareResponse(HttpCode.BadRequest);
        }

        // Non Static Methods
        private PersonId GetPersonId(UserProfileRemoveRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
