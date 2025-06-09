using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.People.ValueObjects.Info;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using System.Text;
using UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonData.Request;
using UseCase.Shared.Dictionaries.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.Addresses;
using UseCase.Shared.Repositories.People;
using UseCase.Shared.Requests.DTOs;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonData
{
    public class UserUpdatePersonDataHandler : IRequestHandler<UserUpdatePersonDataRequest, CommandResponse<UserUpdatePersonDataCommand>>
    {
        // Properties
        private readonly IAddressRepository _addressRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IDictionariesRepository _dictionariesRepository;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public UserUpdatePersonDataHandler(
            IAddressRepository addressRepository,
            IPersonRepository personRepository,
            IDictionariesRepository dictionariesRepository,
            IAuthenticationInspectorService authenticationInspector)
        {
            _addressRepository = addressRepository;
            _personRepository = personRepository;
            _dictionariesRepository = dictionariesRepository;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<CommandResponse<UserUpdatePersonDataCommand>> Handle(UserUpdatePersonDataRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var notFoundSkillIds = await NotFoundSkillIdsAsync(request);
            var notFoundUrlType = await NotFoundUrlTypeIdsAsync(request);
            if (notFoundSkillIds.Any() || notFoundUrlType.Any())
            {
                var stringBuilder = new StringBuilder();
                if (notFoundSkillIds.Any())
                {
                    stringBuilder.AppendLine($"Not Found {nameof(UserUpdatePersonDataCommand.SkillsIds)}: {string.Join(", ", notFoundSkillIds)}");
                }
                if (notFoundUrlType.Any())
                {
                    stringBuilder.AppendLine($"Not Found {nameof(UserUpdatePersonDataCommand.Urls)}: {string.Join(", ", notFoundUrlType)}");
                }
                return PrepareResponse(HttpCode.BadRequest, request.Command, stringBuilder.ToString().Trim());
            }

            var selectResult = await _personRepository.GetAsync(personId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                throw new UseCaseLayerException("Problem with");
            }
            var domainPerson = selectResult.Item;


            var updater = await PrepareUpdaterAsync(domainPerson, request);
            if (updater.HasErrors())
            {
                return PrepareResponse(HttpCode.BadRequest, request.Command, updater.GetErrors());
            }

            var updateResult = await _personRepository.UpdateAsync(updater.Build(), cancellationToken);
            return PrepareResponse(updateResult.Code, request.Command, updateResult.Metadata.Message);
        }

        // Static Methods
        private static CommandResponse<UserUpdatePersonDataCommand> PrepareResponse(
            HttpCode code,
            UserUpdatePersonDataCommand command,
            string? message = null)
        {
            return CommandResponse<UserUpdatePersonDataCommand>.PrepareResponse(code, command, message);
        }

        // Non Static Methods
        private PersonId GetPersonId(UserUpdatePersonDataRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private async Task<Guid> GetaddressIdAsync(AddressRequestDto address)
        {
            return await _addressRepository.CreateAddressAsync(address);
        }

        private async Task<IEnumerable<int>> NotFoundSkillIdsAsync(UserUpdatePersonDataRequest request)
        {
            var skillsDictionary = await _dictionariesRepository.GetSkillsAsync();
            var skillsIds = skillsDictionary.Keys.ToHashSet();
            var inputSkillsIds = request.Command.SkillsIds.ToHashSet();
            return inputSkillsIds.Except(skillsIds);
        }


        private async Task<IEnumerable<int>> NotFoundUrlTypeIdsAsync(UserUpdatePersonDataRequest request)
        {
            var urlTypesDictionary = await _dictionariesRepository.GetUrlTypesAsync();
            var urlTypesIds = urlTypesDictionary.Keys.ToHashSet();
            var inputUrlTypeIds = request.Command.Urls
                .Select(x => x.UrlTypeId)
                .ToHashSet();
            return inputUrlTypeIds.Except(urlTypesIds);
        }

        private async Task<DomainPerson.Updater> PrepareUpdaterAsync(DomainPerson domain, UserUpdatePersonDataRequest request)
        {
            var adaptedBirthDate = request.Command.BirthDate.HasValue ?
                CustomTimeProvider.GetDateOnly(request.Command.BirthDate.Value)
                : (DateOnly?)null;

            var uprater = new DomainPerson.Updater(domain)
                .SetDescription(request.Command.Description)
                .SetContactEmail(request.Command.ContactEmail)
                .SetContactPhoneNumber(request.Command.ContactPhoneNumber)
                .SetBirthDate(adaptedBirthDate)
                .SetHasTwoFactorAuthentication(request.Command.IsTwoFactorAuthentication)
                .SetIsStudent(request.Command.IsStudent)
                .SetSkills(request.Command.SkillsIds.Select(skillId => (PersonSkillInfo)skillId))
                .SetUrls(request.Command.Urls.Select(url => PersonUrlInfo.Prepare(url.UrlTypeId, url.Value)));

            if (request.Command.Address == null)
            {
                uprater.SetAddressId(null);
            }
            else
            {
                var newAddressId = await GetaddressIdAsync(request.Command.Address);
                uprater.SetAddressId(newAddressId);
            }
            return uprater;
        }
    }
}