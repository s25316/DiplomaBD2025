using Domain.Features.People.DomainEvents.ProfileEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents;
using UseCase.Shared.Interfaces;
using UseCase.Shared.Services;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.DomainEvents.PersonProfileCreated
{
    public class PersonProfileCreatedEventHandler : INotificationHandler<PersonProfileCreatedEvent>
    {
        private const string TITTLE = "[Activation profile]";
        private const string TEXT = "For activation profile kick link below:";

        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IKafkaService _kafkaService;
        private readonly IEmailService _emailService;


        // Constructor
        public PersonProfileCreatedEventHandler(
            IAuthenticationGeneratorService authenticationGenerator,
            IKafkaService kafkaService,
            IEmailService emailService)
        {
            _authenticationGenerator = authenticationGenerator;
            _kafkaService = kafkaService;
            _emailService = emailService;
        }


        // Methods
        public async Task Handle(PersonProfileCreatedEvent notification, CancellationToken cancellationToken)
        {
            notification.UrlSegment = _authenticationGenerator.GenerateUrlSegment();

            var url = $"{Configuration.UrlProfileActivation}/{notification.UserId}/{notification.UrlSegment}";
            await _emailService.SendAsync(
                notification.Email,
                TITTLE,
                $"{TEXT}\n{url}",
                cancellationToken);

            var mongoEvent = (UserProfileCreatedMongoDb)notification;
            await _kafkaService.SendUserLogAsync(mongoEvent, cancellationToken);
        }
    }
}
