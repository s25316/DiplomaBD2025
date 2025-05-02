using Domain.Features.People.DomainEvents;
using MediatR;
using UseCase.Kafka;
using UseCase.MongoDb.UserLogs.Models.UserEvents.AuthenticationEvents;

namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.DomainEvents.PersonAuthorization2Stage
{
    public class PersonAuthorization2StageEventHandler : INotificationHandler<PersonAuthorization2StageEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonAuthorization2StageEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonAuthorization2StageEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserAuthorization2StageMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
