using MediatR;
using UseCase.Roles.Guests.Queries.Dictionaries.GetNotificationTypes.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetNotificationTypes
{
    public class GetNotificationTypesHandler : IRequestHandler<GetNotificationTypesRequest, IEnumerable<NotificationTypeDto>>
    {
        // Properties
        private readonly IDictionariesRepository _dictionariesRepository;


        // Constructor
        public GetNotificationTypesHandler(IDictionariesRepository dictionariesRepository)
        {
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<IEnumerable<NotificationTypeDto>> Handle(GetNotificationTypesRequest request, CancellationToken cancellationToken)
        {
            var dictionary = await _dictionariesRepository.GetNotificationTypesAsync();
            return dictionary.Values.OrderBy(nType => nType.NotificationTypeId);
        }
    }
}
