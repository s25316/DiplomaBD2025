using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Response;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData
{
    public class UserProfileSetBaseDataHandler : IRequestHandler<UserProfileSetBaseDataRequest, UserProfileSetBaseDataResponse>
    {
        // Properties
        private readonly IPersonRepository _personRepository;
        private readonly IAuthenticationInspectorService _inspectorService;


        // Constructor
        public UserProfileSetBaseDataHandler(
            IPersonRepository personRepository,
            IAuthenticationInspectorService inspectorService)
        {
            _inspectorService = inspectorService;
            _personRepository = personRepository;
        }


        public async Task<UserProfileSetBaseDataResponse> Handle(UserProfileSetBaseDataRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _personRepository.GetAsync(personId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                throw new UseCaseLayerException("Problem with JWT Generator");
            }
            var doaminPerson = selectResult.Item;

            if (!doaminPerson.IsNotCompleteProfile)
            {
                return PrepareResponse(HttpCode.Conflict, request.Command);
            }


            var updater = PrepareUpdater(doaminPerson, request);
            if (updater.HasErrors())
            {
                return PrepareResponse(HttpCode.BadRequest, request.Command, updater.GetErrors());
            }
            var updateResult = await _personRepository.UpdateAsync(doaminPerson, cancellationToken);
            return PrepareResponse(updateResult.Code, request.Command, updateResult.Metadata.Message);
        }

        // Static Methods
        private static UserProfileSetBaseDataResponse PrepareResponse(
            HttpCode code,
            UserProfileSetBaseDataCommand command,
            string? message = null)
        {
            return UserProfileSetBaseDataResponse.PrepareResponse(code, command, message);
        }

        private static DomainPerson.Updater PrepareUpdater(
            DomainPerson domain,
            UserProfileSetBaseDataRequest request)
        {
            var updater = new DomainPerson.Updater(domain)
                .SetName(request.Command.Name)
                .SetSurname(request.Command.Surname);

            if (request.Command.BirthDate.HasValue)
            {
                var birthDateDateOnly = CustomTimeProvider.GetDateOnly(request.Command.BirthDate.Value);
                updater.SetBirthDate(birthDateDateOnly);
            }

            return updater;
        }

        // Non Static Methods
        private PersonId GetPersonId(UserProfileSetBaseDataRequest request)
        {
            return _inspectorService.GetPersonId(request.Metadata.Claims);
        }
    }
}
