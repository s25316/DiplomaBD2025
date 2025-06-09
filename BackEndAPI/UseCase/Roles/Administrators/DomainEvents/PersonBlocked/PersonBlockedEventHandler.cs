using Domain.Features.People.DomainEvents.BlockingEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Administrators.DomainEvents.PersonBlocked
{
    public class PersonBlockedEventHandler : INotificationHandler<PersonBlockedEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonBlockedEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonBlockedEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserProfileBlockedMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
