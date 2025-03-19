using MediatR;
using UseCase.Shared.Dictionaries.GetFaqs.Request;
using UseCase.Shared.Dictionaries.GetFaqs.Response;
using UseCase.Shared.Dictionaries.Repositories;

namespace UseCase.Shared.Dictionaries.GetFaqs
{
    public class GetFaqsHandler : IRequestHandler<GetFaqsRequest, IEnumerable<FaqDto>>
    {
        // Properties 
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetFaqsHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<FaqDto>> Handle(GetFaqsRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetFaqsAsync();
            return dictionary.Values.OrderBy(x => x.Created);
        }
    }
}
