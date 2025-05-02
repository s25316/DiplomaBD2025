using Domain.Features.People.DomainEvents;
using MediatR;
using UseCase.Kafka;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.DomainEvents.PersonProfileActivated
{
    public class PersonProfileActivatedHandler : INotificationHandler<PersonProfileActivatedEvent>
    {
        // Properties
        private readonly IKafkaService _kafka;


        // Constructor
        public PersonProfileActivatedHandler(
            IKafkaService kafka)
        {
            _kafka = kafka;
        }


        // Methods
        public async Task Handle(PersonProfileActivatedEvent notification, CancellationToken cancellationToken)
        {
            var mongoEvent = (UserProfileActivatedMongoDb)notification;
            await _kafka.SendUserLogAsync(mongoEvent, cancellationToken);
        }
    }
}
