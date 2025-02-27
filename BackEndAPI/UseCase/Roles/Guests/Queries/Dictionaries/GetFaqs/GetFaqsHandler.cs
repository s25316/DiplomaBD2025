using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetFaqs.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetFaqs
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
            return dictionary.Values.OrderBy(faq => faq.Created);
        }
    }
}
