using Domain.Features.People.DomainEvents.AuthorizationEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Users.DomainEvents.PersonAuthorizationLogOut
{
    public class PersonAuthorizationLogOutEventHandler : INotificationHandler<PersonAuthorizationLogOutEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonAuthorizationLogOutEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }

        // Methods
        public async Task Handle(PersonAuthorizationLogOutEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserAuthorizationLogOutMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
