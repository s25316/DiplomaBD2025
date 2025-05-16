using Domain.Features.People.DomainEvents.ProfileEvents;
using Domain.Shared.CustomProviders;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents;
using UseCase.Shared.Interfaces;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.DomainEvents.PersonProfileInitiateResetPassword
{
    class PersonProfileInitiateResetPasswordEventHandler : INotificationHandler<PersonProfileInitiateResetPasswordEvent>
    {
        // Properties
        private static int _timeMinutesValid = 5;
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IKafkaService _kafkaService;

        // Constructor
        public PersonProfileInitiateResetPasswordEventHandler(
            IKafkaService kafkaService,
            IAuthenticationGeneratorService authenticationGenerator)
        {
            _kafkaService = kafkaService;
            _authenticationGenerator = authenticationGenerator;
        }

        // Methods
        public async Task Handle(PersonProfileInitiateResetPasswordEvent notification, CancellationToken cancellationToken)
        {
            notification.UrlSegment = _authenticationGenerator.GenerateUrlSegment();
            notification.ValidTo = CustomTimeProvider.Now.AddMinutes(_timeMinutesValid);

            var log = (UserProfileInitiatedResetPasswordMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
