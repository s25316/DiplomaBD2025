using Domain.Features.People.DomainEvents.AuthorizationEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Users.DomainEvents.PersonAuthorizationLoginIn
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
