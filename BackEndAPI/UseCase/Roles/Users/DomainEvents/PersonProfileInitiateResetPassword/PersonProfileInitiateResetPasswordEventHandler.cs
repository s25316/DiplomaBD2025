using Domain.Features.People.DomainEvents.ProfileEvents;
using Domain.Shared.CustomProviders;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents;
using UseCase.Shared.Interfaces;
using UseCase.Shared.Services;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.DomainEvents.PersonProfileInitiateResetPassword
{
    class PersonProfileInitiateResetPasswordEventHandler : INotificationHandler<PersonProfileInitiateResetPasswordEvent>
    {
        // Properties
        private const int TIME_MINUTES_VALID = 5;
        private const string TITTLE = "[Reset password]";
        private const string TEXT = "For reseting password kick link below:";

        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IKafkaService _kafkaService;
        private readonly IEmailService _emailService;

        // Constructor
        public PersonProfileInitiateResetPasswordEventHandler(
            IEmailService emailService,
            IKafkaService kafkaService,
            IAuthenticationGeneratorService authenticationGenerator)
        {
            _kafkaService = kafkaService;
            _emailService = emailService;
            _authenticationGenerator = authenticationGenerator;
        }

        // Methods
        public async Task Handle(PersonProfileInitiateResetPasswordEvent notification, CancellationToken cancellationToken)
        {
            notification.UrlSegment = _authenticationGenerator.GenerateUrlSegment();
            notification.ValidTo = CustomTimeProvider.Now.AddMinutes(TIME_MINUTES_VALID);

            var url = $"{Configuration.UrlResetPassword}/{notification.UserId}/{notification.UrlSegment}";
            await _emailService.SendAsync(
                notification.Email,
                TITTLE,
                $"{TEXT}\n{url}",
                cancellationToken);

            var log = (UserProfileInitiatedResetPasswordMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
