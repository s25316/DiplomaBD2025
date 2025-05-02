using Domain.Features.People.DomainEvents;
using MediatR;
using UseCase.Kafka;
using UseCase.MongoDb.UserLogs.Models.UserEvents.AuthenticationEvents;

namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.DomainEvents.PersonAuthorizationLoginIn
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
