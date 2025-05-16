using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonLogin.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonLogin
{
    public class UserUpdatePersonLoginHandler : IRequestHandler<UserUpdatePersonLoginRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IPersonRepository _personRepository;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public UserUpdatePersonLoginHandler(
            IPersonRepository personRepository,
            IAuthenticationInspectorService authenticationInspector)
        {
            _personRepository = personRepository;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserUpdatePersonLoginRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _personRepository.GetAsync(personId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                throw new UseCaseLayerException(
                    $"Problem With Authorization: {selectResult.Metadata.Message}");
            }

            // Domain Part
            var updater = PrepaerUpdater(selectResult.Item, request);
            if (updater.HasErrors())
            {
                return PrepareInvalid(updater.GetErrors());
            }

            var updateResult = await _personRepository
                .UpdateAsync(updater.Build(), cancellationToken);
            if (updateResult.Code != HttpCode.Ok)
            {
                return PrepareInvalid(updateResult.Metadata.Message);
            }
            return PrepareValid();
        }

        // Static Methods
        private static DomainPerson.Updater PrepaerUpdater(
            DomainPerson domainPerson,
            UserUpdatePersonLoginRequest request)
        {
            return new DomainPerson.Updater(domainPerson)
                .SetLogin(request.Command.NewLogin);
        }

        public static ResultMetadataResponse PrepareValid()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.Ok);
        }

        public static ResultMetadataResponse PrepareInvalid(string? message = null)
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.BadRequest, message);
        }

        // Non Static Methods
        private PersonId GetPersonId(UserUpdatePersonLoginRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
