using Domain.Features.People.DomainEvents.ProfileEvents;
using Domain.Shared.CustomProviders;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;
using UseCase.Shared.Interfaces;
using UseCase.Shared.Services;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.DomainEvents.PersonProfileRemoved
{
    public class PersonProfileRemovedEventHandler : INotificationHandler<PersonProfileRemovedEvent>
    {
        // Properties
        private const int TIME_DAYS_VALID = 30;
        private const string TITTLE = "[Restore Account]";
        private const string TEXT = "For restore account kick link below till";

        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IKafkaService _kafkaService;
        private readonly IEmailService _emailService;


        // Constructor
        public PersonProfileRemovedEventHandler(
            IEmailService emailService,
            IKafkaService kafkaService,
            IAuthenticationGeneratorService authenticationGenerator)
        {
            _emailService = emailService;
            _kafkaService = kafkaService;
            _authenticationGenerator = authenticationGenerator;
        }

        // Methods
        public async Task Handle(PersonProfileRemovedEvent notification, CancellationToken cancellationToken)
        {
            notification.UrlSegment = _authenticationGenerator.GenerateUrlSegment();
            notification.ValidTo = CustomTimeProvider.GetDateTime(
                CustomTimeProvider.Today.AddDays(TIME_DAYS_VALID));

            var text = $"{TEXT} {notification.ValidTo}:";
            var url = $"{Configuration.UrlProfileRestore}/{notification.UserId}/{notification.UrlSegment}";

            await _emailService.SendAsync(
                notification.Email,
                TITTLE,
                $"{text}\n{url}",
                cancellationToken);

            var log = (UserProfileRemovedMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
