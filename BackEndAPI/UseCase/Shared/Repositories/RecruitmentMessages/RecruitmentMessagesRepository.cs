using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.Recruitments.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.Requests.QueryParameters;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Shared.Repositories.RecruitmentMessages
{
    public class RecruitmentMessagesRepository : IRecruitmentMessagesRepository
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;


        // Constructor
        public RecruitmentMessagesRepository(
            IMapper mapper,
             DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task<ResultMetadataResponse> CreateAsync(
            PersonId personId,
            RecruitmentId recruitmentId,
            string message,
            bool isPerson,
            CancellationToken cancellationToken)
        {
            var recruitmentIdValue = recruitmentId.Value;
            var personIdValue = personId.Value;

            var selectResult = await _context.HrProcesses
                .Where(recruitment => recruitment.ProcessId == recruitmentIdValue)
                .Select(recruitment => new
                {
                    Recruitment = recruitment,
                    RoleCount = _context.CompanyPeople
                        .Where(role =>
                            role.Deny == null &&
                            role.PersonId == personIdValue)
                        .Count(role =>
                            _context.OfferConnections
                            .Include(x => x.OfferTemplate)
                            .Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == recruitment.OfferId &&
                                role.CompanyId == oc.OfferTemplate.CompanyId
                        )),
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (selectResult == null)
            {
                return ResultMetadataResponse.PrepareResponse(HttpCode.NotFound);
            }
            if ((!isPerson && selectResult.RoleCount == 0) ||
                (isPerson && selectResult.Recruitment.PersonId != personIdValue))
            {
                return ResultMetadataResponse.PrepareResponse(HttpCode.Forbidden);
            }

            var chatMessage = new Hrchat
            {
                ProcessId = recruitmentId,
                IsPersonSend = isPerson,
                Message = message,
                Created = CustomTimeProvider.Now,
            };
            await _context.Hrchats.AddAsync(chatMessage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ResultMetadataResponse.PrepareResponse(HttpCode.Created);
        }

        public async Task<ItemsResponse<MessageDto>> GetAsync(
            PersonId personId,
            RecruitmentId recruitmentId,
            bool isPerson,
            PaginationQueryParametersDto pagination,
            bool ascending,
            CancellationToken cancellationToken)
        {
            var recruitmentIdValue = recruitmentId.Value;
            var personIdValue = personId.Value;

            var messagesQuery = ascending
                ? _context.Hrchats
                    .Where(chat => chat.ProcessId == recruitmentIdValue)
                    .Paginate(pagination)
                    .OrderBy(chat => chat.Created)
                : _context.Hrchats
                    .Where(chat => chat.ProcessId == recruitmentIdValue)
                    .Paginate(pagination)
                    .OrderByDescending(chat => chat.Created);

            var selectResult = await _context.HrProcesses
                .Where(recruitment => recruitment.ProcessId == recruitmentIdValue)
                .Select(recruitment => new
                {
                    Recruitment = recruitment,
                    RoleCount = _context.CompanyPeople
                        .Where(role =>
                            role.Deny == null &&
                            role.PersonId == personIdValue)
                        .Count(role =>
                            _context.OfferConnections
                            .Include(x => x.OfferTemplate)
                            .Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == recruitment.OfferId &&
                                role.CompanyId == oc.OfferTemplate.CompanyId
                        )),
                    Messages = messagesQuery.ToList(),
                    TotalCount = _context.Hrchats
                        .Where(chat => chat.ProcessId == recruitment.ProcessId)
                        .Count()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (selectResult == null)
            {
                return ItemsResponse<MessageDto>.PrepareResponse(HttpCode.NotFound, [], 0);
            }
            if ((!isPerson && selectResult.RoleCount == 0) ||
                (isPerson && selectResult.Recruitment.PersonId != personIdValue))
            {
                return ItemsResponse<MessageDto>.PrepareResponse(HttpCode.Forbidden, [], 0);
            }

            var isChangedRead = false;
            foreach (var message in selectResult.Messages)
            {
                if (!message.Read.HasValue && message.IsPersonSend != isPerson)
                {
                    if (!isChangedRead)
                    {
                        isChangedRead = true;
                    }
                    message.Read = CustomTimeProvider.Now;
                }
            }
            if (isChangedRead)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }

            return ItemsResponse<MessageDto>.PrepareResponse(
                HttpCode.Ok,
                _mapper.Map<IEnumerable<MessageDto>>(selectResult.Messages),
                selectResult.TotalCount);
        }
    }
}
