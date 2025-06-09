using Domain.Features.People.DomainEvents.BlockingEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Administrators.DomainEvents.PersonUnBlocked
{
    public class PersonUnBlockedEventHandler : INotificationHandler<PersonUnBlockedEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonUnBlockedEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonUnBlockedEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserProfileUnBlockedMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
