using MediatR;

namespace UseCase.Roles.People.Queries
{
    class GetUserQueryHandler : IRequestHandler<MyQuery, MyResp>
    {

        public GetUserQueryHandler()
        {

        }

        public async Task<MyResp> Handle(MyQuery request, CancellationToken cancellationToken)
        {
            var count = 1; //_context.People.CountAsync(cancellationToken);
            return new MyResp
            {
                Count = count,
            };
        }
    }
}
