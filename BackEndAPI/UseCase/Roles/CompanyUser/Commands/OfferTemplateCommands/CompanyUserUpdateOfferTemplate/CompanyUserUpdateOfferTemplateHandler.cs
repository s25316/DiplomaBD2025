using Domain.Features.OfferTemplates.ValueObjects.Info;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserUpdateOfferTemplate.Request;
using UseCase.Roles.CompanyUser.Repositories.OfferTemplates;
using UseCase.Shared.Dictionaries.Repositories;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Aggregates.OfferTemplate;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserUpdateOfferTemplate
{
    public class CompanyUserUpdateOfferTemplateHandler : IRequestHandler<CompanyUserUpdateOfferTemplateRequest, CommandResponse<CompanyUserUpdateOfferTemplateCommand>>
    {
        // Property
        private readonly IAuthenticationInspectorService _inspectorService;
        private readonly IDictionariesRepository _dictionariesRepository;
        private readonly IOfferTemplateRepository _offerTemplateRepository;


        // Constructor
        public CompanyUserUpdateOfferTemplateHandler(
            IAuthenticationInspectorService inspectorService,
            IDictionariesRepository dictionariesRepository,
            IOfferTemplateRepository offerTemplateRepository)
        {
            _inspectorService = inspectorService;
            _dictionariesRepository = dictionariesRepository;
            _offerTemplateRepository = offerTemplateRepository;
        }


        // Methods
        public async Task<CommandResponse<CompanyUserUpdateOfferTemplateCommand>> Handle(CompanyUserUpdateOfferTemplateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _offerTemplateRepository.GetAsync(
                personId,
                request.OfferTemplateId,
                cancellationToken);

            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(
                    selectResult.Code,
                    selectResult.Metadata.Message,
                    request);
            }

            var doaminTemplate = selectResult.Item;
            var notFoundSkillIds = await CheckSkillIdsAsync(request);
            if (notFoundSkillIds.Any())
            {
                return PrepareResponse(
                        HttpCode.NotFound,
                        $"{Messages.Entity_Branch_SkillIds_NotFound}: {string.Join(", ", notFoundSkillIds)}",
                        request);
            }

            var updater = Update(doaminTemplate, request);
            if (updater.HasErrors())
            {
                return PrepareResponse(
                        HttpCode.BadRequest,
                        updater.GetErrors(),
                        request);
            }

            var updateResult = await _offerTemplateRepository.UpdateAsync(
               personId,
               updater.Build(),
               cancellationToken);

            // If Ok or Not
            return PrepareResponse(
                   updateResult.Code,
                   updateResult.Metadata.Message,
                   request);
        }


        // Private Static Methods
        private static CommandResponse<CompanyUserUpdateOfferTemplateCommand> PrepareResponse(
            HttpCode code,
            string? message,
            CompanyUserUpdateOfferTemplateRequest request)
        {
            return CommandResponse<CompanyUserUpdateOfferTemplateCommand>.PrepareResponse(
                code,
                request.Command,
                message);
        }

        private static DomainOfferTemplate.Updater Update(
            DomainOfferTemplate item,
            CompanyUserUpdateOfferTemplateRequest request)
        {
            var updater = new DomainOfferTemplate.Updater(item);

            if (!string.IsNullOrWhiteSpace(request.Command.Name))
            {
                updater.SetName(request.Command.Name);
            }
            if (!string.IsNullOrWhiteSpace(request.Command.Description))
            {
                updater.SetDescription(request.Command.Description);
            }

            updater.SetSkills(request.Command.Skills
                .Select(s => new OfferSkillInfo
                {
                    Id = null,
                    SkillId = s.SkillId,
                    IsRequired = s.IsRequired,
                    Created = null,
                }));

            return updater;
        }

        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUserUpdateOfferTemplateRequest request)
        {
            return _inspectorService.GetPersonId(request.Metadata.Claims);
        }

        private async Task<IEnumerable<int>> CheckSkillIdsAsync(CompanyUserUpdateOfferTemplateRequest request)
        {
            if (!request.Command.Skills.Any())
            {
                return [];
            }

            var skillsDictionary = await _dictionariesRepository.GetSkillsAsync();
            var notFoundSkillIds = new List<int>();
            var commandSkillIds = request.Command.Skills.
                Select(s => s.SkillId)
                .ToHashSet();

            foreach (var id in commandSkillIds)
            {
                if (!skillsDictionary.ContainsKey(id))
                {
                    notFoundSkillIds.Add(id);
                }
            }
            return notFoundSkillIds;
        }
    }
}
