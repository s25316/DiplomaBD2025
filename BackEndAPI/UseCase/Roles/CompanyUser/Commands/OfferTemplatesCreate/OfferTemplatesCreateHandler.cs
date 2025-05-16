using Domain.Features.OfferTemplates.ValueObjects.Info;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using System.Text;
using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response;
using UseCase.Roles.CompanyUser.Repositories.OfferTemplates;
using UseCase.Shared.Dictionaries.GetSkills.Response;
using UseCase.Shared.Dictionaries.Repositories;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Aggregates.OfferTemplate;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate
{
    public class OfferTemplatesCreateHandler : IRequestHandler<OfferTemplatesCreateRequest, OfferTemplatesCreateResponse>
    {
        // Property
        private readonly IAuthenticationInspectorService _inspectorService;
        private readonly IDictionariesRepository _dictionariesRepository;
        private readonly IOfferTemplateRepository _offerTemplateRepository;


        // Constructor
        public OfferTemplatesCreateHandler(
            IAuthenticationInspectorService inspectorService,
            IDictionariesRepository dictionariesRepository,
            IOfferTemplateRepository offerTemplateRepository)
        {
            _inspectorService = inspectorService;
            _dictionariesRepository = dictionariesRepository;
            _offerTemplateRepository = offerTemplateRepository;
        }


        // Methods
        public async Task<OfferTemplatesCreateResponse> Handle(OfferTemplatesCreateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var skillDictionary = await _dictionariesRepository.GetSkillsAsync();
            var buildResult = Build(request, skillDictionary);

            if (!buildResult.IsValid)
            {
                return new OfferTemplatesCreateResponse
                {
                    Result = buildResult.Dictionary
                        .Select(pair => new BaseCommandResult<OfferTemplateCreateCommand>
                        {
                            Item = pair.Key,
                            IsCorrect = string.IsNullOrWhiteSpace(pair.Value.Error),
                            Message = pair.Value.Error ?? string.Empty,
                        }),
                    HttpCode = HttpCode.BadRequest,
                };
            }

            var dictionary = buildResult.Dictionary.ToDictionary(
                val => val.Key,
                val => val.Value.OfferTemplate);
            var repositoryResult = await _offerTemplateRepository.CreateAsync(
                personId,
                dictionary.Values,
                cancellationToken);

            return new OfferTemplatesCreateResponse
            {
                Result = dictionary
                        .Select(pair => new BaseCommandResult<OfferTemplateCreateCommand>
                        {
                            Item = pair.Key,
                            IsCorrect = repositoryResult.Dictionary[pair.Value].IsCorrect,
                            Message = repositoryResult.Dictionary[pair.Value].Message,
                        }),
                HttpCode = repositoryResult.Code,
            };
        }

        // Private Static Methods
        private static DomainOfferTemplate.Builder PrepareBuilder(
            Guid companyId,
            OfferTemplateCreateCommand command)
        {
            return new DomainOfferTemplate.Builder()
                .SetCompanyId(companyId)
                .SetName(command.Name)
                .SetDescription(command.Description)
                .SetSkills(command.Skills.Select(skill => new OfferSkillInfo
                {
                    Id = null,
                    SkillId = skill.SkillId,
                    IsRequired = skill.IsRequired,
                    Created = null,
                }));
        }

        private sealed class BuildResult
        {
            public required Dictionary<OfferTemplateCreateCommand, OfferTemplateAndError> Dictionary { get; init; }
            public required bool IsValid { get; init; }
        }

        private sealed class OfferTemplateAndError
        {
            public required DomainOfferTemplate OfferTemplate { get; init; }
            public required string? Error { get; init; }
        }

        private static BuildResult Build(
            OfferTemplatesCreateRequest request,
            IReadOnlyDictionary<int, SkillDto> skillDictionary)
        {
            var stringBuilder = new StringBuilder();
            var notFoundSkillIds = new StringBuilder();
            var dictionary = new Dictionary<OfferTemplateCreateCommand, OfferTemplateAndError>();
            var isValid = true;

            foreach (var command in request.Commands)
            {
                stringBuilder.Clear();
                notFoundSkillIds.Clear();

                var builder = PrepareBuilder(request.CompanyId, command);
                if (builder.HasErrors())
                {
                    stringBuilder.AppendLine(builder.GetErrors());
                }

                foreach (var skillId in command.Skills)
                {
                    if (!skillDictionary.ContainsKey(skillId.SkillId))
                    {
                        notFoundSkillIds.Append($"{skillId.SkillId} ");
                    }
                }

                if (notFoundSkillIds.Length > 0)
                {
                    stringBuilder.AppendLine(
                        $"{Messages.Entity_Branch_SkillIds_NotFound}: {notFoundSkillIds.ToString()}");
                }

                if (isValid && stringBuilder.Length > 0)
                {
                    isValid = false;
                }

                dictionary[command] = new OfferTemplateAndError
                {
                    OfferTemplate = builder.Build(),
                    Error = stringBuilder.Length == 0 ? null : stringBuilder.ToString(),
                };
            }

            return new BuildResult
            {
                Dictionary = dictionary,
                IsValid = isValid
            };
        }

        // Private Non Static Methods
        private PersonId GetPersonId(OfferTemplatesCreateRequest request)
        {
            return _inspectorService.GetPersonId(request.Metadata.Claims);
        }
    }
}
