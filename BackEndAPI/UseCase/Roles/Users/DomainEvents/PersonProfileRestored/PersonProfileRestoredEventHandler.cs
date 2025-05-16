using Domain.Features.People.DomainEvents.ProfileEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Users.DomainEvents.PersonProfileRestored
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
