using Domain.Features.People.DomainEvents.ProfileEvents;
using MediatR;
using UseCase.Kafka;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;

namespace UseCase.Roles.Users.Commands.ProfileCommands.DomainEvents.PersonProfileRestored
{
    public class PersonProfileRestoredEventHandler : INotificationHandler<PersonProfileRestoredEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonProfileRestoredEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonProfileRestoredEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserProfileRestoredMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
