using Domain.Features.OfferTemplates.ValueObjects;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using MediatR;
using System.Text;
using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response;
using UseCase.Roles.CompanyUser.Commands.Repositories.OfferTemplates;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.Commands;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Entities.OfferTemplate;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate
{
    public class OfferTemplatesCreateHandler : IRequestHandler<OfferTemplatesCreateRequest, OfferTemplatesCreateResponse>
    {
        // Properties 
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IDictionariesRepository _dictionariesRepository;
        private readonly IOfferTemplateRepository _offerTemplateRepository;

        // Constructor
        public OfferTemplatesCreateHandler(
            IAuthenticationInspectorService authenticationInspector,
            IDictionariesRepository dictionariesRepository,
            IOfferTemplateRepository offerTemplateRepository)
        {
            _authenticationInspector = authenticationInspector;
            _dictionariesRepository = dictionariesRepository;
            _offerTemplateRepository = offerTemplateRepository;
        }


        // Methods
        public async Task<OfferTemplatesCreateResponse> Handle(OfferTemplatesCreateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var isAllowedOperation = await _offerTemplateRepository.HasAccessToCompany(
                personId,
                request.CompanyId,
                cancellationToken);

            if (!isAllowedOperation)
            {
                return new OfferTemplatesCreateResponse
                {
                    Commands = request.Commands.Select(cmd => new ResponseCommandTemplate<OfferTemplateCommand>
                    {
                        Item = cmd,
                        IsCorrect = false,
                        Message = HttpCode.Forbidden.Description(),
                    }),
                    IsCorrect = false,
                    HttpCode = HttpCode.Forbidden,
                };
            }

            var builders = await GetBuildersAsync(request);
            if (!builders.IsValidRequest)
            {
                return new OfferTemplatesCreateResponse
                {
                    Commands = builders.Dictionary.Select(pair => new ResponseCommandTemplate<OfferTemplateCommand>
                    {
                        Item = pair.Key,
                        IsCorrect = string.IsNullOrWhiteSpace(pair.Value.Error),
                        Message = string.IsNullOrWhiteSpace(pair.Value.Error) ?
                            HttpCode.Correct.Description() :
                            pair.Value.Error,
                    }),
                    IsCorrect = false,
                    HttpCode = HttpCode.BadRequest,
                };
            }

            var list = new List<DomainOfferTemplate>();
            foreach (var pair in builders.Dictionary.Values)
            {
                list.Add(pair.Builder.Build());
            }

            await _offerTemplateRepository.CreateAsync(list, cancellationToken);
            return new OfferTemplatesCreateResponse
            {
                Commands = request.Commands.Select(cmd => new ResponseCommandTemplate<OfferTemplateCommand>
                {
                    Item = cmd,
                    IsCorrect = true,
                    Message = HttpCode.Created.Description(),
                }),
                IsCorrect = true,
                HttpCode = HttpCode.Created
            };
        }

        // Private Methods
        private PersonId GetPersonId(OfferTemplatesCreateRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        // Records for Method below
        private sealed record GetBuildersResult(
            bool IsValidRequest,
            Dictionary<OfferTemplateCommand, BuilderAndError> Dictionary);

        private sealed record BuilderAndError(
            DomainOfferTemplate.Builder Builder,
            string Error);

        private async Task<GetBuildersResult> GetBuildersAsync(OfferTemplatesCreateRequest request)
        {
            var skillsDictionary = await _dictionariesRepository.GetSkillsAsync();
            var dictionary = new Dictionary<OfferTemplateCommand, BuilderAndError>();
            var isValidRequest = true;

            foreach (var command in request.Commands)
            {
                var builder = new DomainOfferTemplate.Builder()
                    .SetName(command.Name)
                    .SetDescription(command.Description)
                    .SetCompanyId(request.CompanyId)
                    .SetSkillsIds(command.Skills.Select(skill => new OfferSkill
                    {
                        SkillId = skill.SkillId,
                        IsRequired = skill.IsRequired,
                    }));

                var notFoundIds = new List<int>();
                foreach (var skill in command.Skills)
                {
                    if (!skillsDictionary.ContainsKey(skill.SkillId))
                    {
                        notFoundIds.Add(skill.SkillId);
                    }
                }

                var errors = new StringBuilder();
                if (builder.HasErrors())
                {
                    errors.AppendLine(builder.GetErrors());
                }
                if (notFoundIds.Any())
                {
                    errors.Append($"Not found {nameof(OfferSkillRequestDto.SkillId)}s : ");
                    errors.Append(string.Join(", ", notFoundIds));
                }
                //Set valid request on false
                if (isValidRequest && errors.Length > 0)
                {
                    isValidRequest = false;
                }

                dictionary[command] = new BuilderAndError(builder, errors.ToString());
            }
            return new GetBuildersResult(isValidRequest, dictionary);
        }

    }
}
