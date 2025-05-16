using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.RemovingCommands.UserRemovePerson.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Commands.RemovingCommands.UserRemovePerson
{
    public class UserRemovePersonHandler : IRequestHandler<UserRemovePersonRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IMediator _mediator;
        private readonly IPersonRepository _personRepository;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public UserRemovePersonHandler(
            IMediator mediator,
            IPersonRepository personRepository,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mediator = mediator;
            _personRepository = personRepository;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserRemovePersonRequest request, CancellationToken cancellationToken)
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
        public static ResultMetadataResponse ValidResponse()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.Ok);
        }

        public static ResultMetadataResponse InValidResponse()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.BadRequest);
        }

        // Non Static Methods
        private PersonId GetPersonId(UserRemovePersonRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
