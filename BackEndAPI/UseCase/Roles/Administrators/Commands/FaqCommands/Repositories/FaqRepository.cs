using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Repositories.BaseEFRepository;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.Repositories
{
    public class FaqRepository : IFaqRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;


        // Constructor
        public FaqRepository(DiplomaBdContext context)
        {
            _context = context;
        }


        // Methods
        public async Task CreateAsync(
            string question,
            string answer,
            CancellationToken cancellationToken)
        {
            var item = new Faq
            {
                Question = question,
                Answer = answer,
                Created = CustomTimeProvider.Now,
            };

            await _context.Faqs.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task<RepositoryRemoveResponse> RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _context.Faqs
                .Where(item => item.FaqId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (item == null)
            {
                return RepositoryRemoveResponse.PrepareResponse(HttpCode.NotFound);
            }
            if (item.Removed.HasValue)
            {
                return RepositoryRemoveResponse.PrepareResponse(
                    HttpCode.Conflict,
                    "Already removed");
            }

            item.Removed = CustomTimeProvider.Now;
            await _context.SaveChangesAsync(cancellationToken);
            return RepositoryRemoveResponse.PrepareResponse(HttpCode.Ok);
        }


        public async Task<RepositoryUpdateResponse> UpdateAsync(
            Guid id,
            string answer,
            CancellationToken cancellationToken)
        {
            var item = await _context.Faqs
                .Where(item => item.FaqId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (item == null)
            {
                return RepositoryUpdateResponse.InvalidResponse(HttpCode.NotFound);
            }
            if (item.Removed.HasValue)
            {
                return RepositoryUpdateResponse.InvalidResponse(
                    HttpCode.Conflict,
                    "Already removed");
            }

            item.Answer = answer;
            await _context.SaveChangesAsync(cancellationToken);
            return RepositoryUpdateResponse.ValidResponse();
        }
    }
}
