using Domain.Features.People.DomainEvents.ProfileEvents;
using MediatR;
using UseCase.Kafka;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.Commands.ProfileCommands.DomainEvents.PersonProfileCreated
{
    public class PersonProfileCreatedEventHandler : INotificationHandler<PersonProfileCreatedEvent>
    {
        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonProfileCreatedEventHandler(
            IAuthenticationGeneratorService authenticationGenerator,
            IKafkaService kafkaService)
        {
            _authenticationGenerator = authenticationGenerator;
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonProfileCreatedEvent notification, CancellationToken cancellationToken)
        {
            notification.UrlSegment = _authenticationGenerator.GenerateUrlSegment();
            var mongoEvent = (UserProfileCreatedMongoDb)notification;
            await _kafkaService.SendUserLogAsync(mongoEvent, cancellationToken);
        }
    }
}
