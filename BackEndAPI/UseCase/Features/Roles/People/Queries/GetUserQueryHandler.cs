using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;

namespace UseCase.Features.Roles.People.Queries
{
    class GetUserQueryHandler : IRequestHandler<MyQuery, MyResp>
    {
        private readonly DiplomaBdContext _context;

        public GetUserQueryHandler(DiplomaBdContext context)
        {
            _context = context;
        }

        public async Task<MyResp> Handle(MyQuery request, CancellationToken cancellationToken)
        {
            var count = await _context.People.CountAsync(cancellationToken);
            return new MyResp
            {
                Count = count,
            };
        }
    }
}
