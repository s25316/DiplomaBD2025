// Ignore Spelling: redis

using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetSalaryTerms.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetSalaryTerms
{
    public class GetSalaryTermsHandler : IRequestHandler<GetSalaryTermsRequest, IEnumerable<SalaryTermDto>>
    {
        // Properties
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetSalaryTermsHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<SalaryTermDto>> Handle(GetSalaryTermsRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetSalaryTermsAsync();
            return dictionary.Values.OrderBy(value => value.SalaryTermId);
        }
    }
}
