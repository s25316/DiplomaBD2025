using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.UpdatingCommands.UserSetBasePersonData.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserSetBasePersonData
{
    public class UserSetBasePersonDataHandler : IRequestHandler<UserSetBasePersonDataRequest, CommandResponse<UserSetBasePersonDataCommand>>
    {
        // Properties
        private readonly IPersonRepository _personRepository;
        private readonly IAuthenticationInspectorService _inspectorService;


        // Constructor
        public UserSetBasePersonDataHandler(
            IPersonRepository personRepository,
            IAuthenticationInspectorService inspectorService)
        {
            _inspectorService = inspectorService;
            _personRepository = personRepository;
        }


        public async Task<CommandResponse<UserSetBasePersonDataCommand>> Handle(UserSetBasePersonDataRequest request, CancellationToken cancellationToken)
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
        private static CommandResponse<UserSetBasePersonDataCommand> PrepareResponse(
            HttpCode code,
            UserSetBasePersonDataCommand command,
            string? message = null)
        {
            return CommandResponse<UserSetBasePersonDataCommand>.PrepareResponse(code, command, message);
        }

        private static DomainPerson.Updater PrepareUpdater(
            DomainPerson domain,
            UserSetBasePersonDataRequest request)
        {
            var updater = new DomainPerson.Updater(domain)
                .SetName(request.Command.Name)
                .SetSurname(request.Command.Surname)
                .SetContactEmail(request.Command.ContactEmail);

            if (request.Command.BirthDate.HasValue)
            {
                var birthDateDateOnly = CustomTimeProvider.GetDateOnly(request.Command.BirthDate.Value);
                updater.SetBirthDate(birthDateDateOnly);
            }

            return updater;
        }

        // Non Static Methods
        private PersonId GetPersonId(UserSetBasePersonDataRequest request)
        {
            return _inspectorService.GetPersonId(request.Metadata.Claims);
        }
    }
}
