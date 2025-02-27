using Domain.Features.People.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Services.Time;
using UseCase.Shared.Templates.Requests;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate
{
    public class OfferTemplatesCreateHandler : IRequestHandler<OfferTemplatesCreateRequest, OfferTemplatesCreateResponse>
    {
        // Properties 
        private readonly DiplomaBdContext _context;
        private readonly ITimeService _timeService;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IDictionariesRepository _dictionariesRepository;

        // Constructor
        public OfferTemplatesCreateHandler(
            DiplomaBdContext context,
            ITimeService timeService,
            IAuthenticationInspectorService authenticationInspector,
            IDictionariesRepository dictionariesRepository)
        {
            _context = context;
            _timeService = timeService;
            _authenticationInspector = authenticationInspector;
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<OfferTemplatesCreateResponse> Handle(OfferTemplatesCreateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request.Metadata);
            var hasAccess = await HasAccessToCompanyAsync(
                request.CompanyId,
                personId,
                cancellationToken);

            if (!hasAccess)
            {
                return new OfferTemplatesCreateResponse
                {
                    Commands = request.Commands.Select(cmd => new BaseResponseGeneric<OfferTemplateCommand>
                    {
                        Item = cmd,
                        IsSuccess = false,
                        Message = "Forbidden",
                    })
                };
            }
            var now = Now();
            var skillsDictionary = await _dictionariesRepository.GetSkillsAsync();
            var responseList = new List<BaseResponseGeneric<OfferTemplateCommand>>();
            var offerTemplates = new List<OfferTemplate>();
            var offerSkills = new List<OfferSkill>();
            foreach (var command in request.Commands)
            {
                var idsNotFound = new List<int>();
                foreach (var skillId in command.Skills.Select(skill => skill.SkillId))
                {
                    if (!skillsDictionary.ContainsKey(skillId))
                    {
                        idsNotFound.Add(skillId);
                    }
                }
                if (idsNotFound.Any())
                {
                    responseList.Add(new BaseResponseGeneric<OfferTemplateCommand>
                    {
                        Item = command,
                        IsSuccess = false,
                        Message = $"Invalid Skill Ids: {string.Join(", ", idsNotFound)}"
                    });
                    continue;
                }
                var template = new OfferTemplate
                {
                    CompanyId = request.CompanyId,
                    Name = command.Name,
                    Description = command.Description,
                    Created = now,
                };
                var skills = command.Skills.Select(skill => new OfferSkill
                {
                    SkillId = skill.SkillId,
                    IsRequired = skill.IsRequired,
                    Created = now,
                    OfferTemplate = template,
                });
                offerTemplates.Add(template);
                offerSkills.AddRange(skills);
                responseList.Add(new BaseResponseGeneric<OfferTemplateCommand>
                {
                    Item = command,
                    IsSuccess = true,
                    Message = $"Success"
                });
            }

            await _context.OfferSkills.AddRangeAsync(offerSkills, cancellationToken);
            await _context.OfferTemplates.AddRangeAsync(offerTemplates, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new OfferTemplatesCreateResponse
            {
                Commands = responseList,
            };
        }

        private async Task<bool> HasAccessToCompanyAsync(
            Guid companyId,
            PersonId personId,
            CancellationToken cancellationToken)
        {
            var roleIds = await _context.CompanyPeople.Where(cp =>
                cp.PersonId == personId.Value &&
                cp.CompanyId == companyId &&
                cp.Deny == null)
                .Select(cp => cp.RoleId)
                .ToListAsync(cancellationToken);
            return roleIds.Any();
        }

        private PersonId GetPersonId(RequestMetadata metadata)
        {
            return _authenticationInspector.GetPersonId(metadata.Claims);
        }

        private DateTime Now()
        {
            return _timeService.GetNow();
        }
    }
}
