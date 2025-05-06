using Domain.Features.People.DomainEvents.AuthorizationEvents;
using MediatR;
using UseCase.Kafka;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.DomainEvents.PersonAuthorizationLoginIn
{
    public class PersonAuthorizationLoginInEventHandler : INotificationHandler<PersonAuthorizationLoginInEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonAuthorizationLoginInEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }

        // Methods
        public async Task Handle(PersonAuthorizationLoginInEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserAuthorizationLoginInMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
