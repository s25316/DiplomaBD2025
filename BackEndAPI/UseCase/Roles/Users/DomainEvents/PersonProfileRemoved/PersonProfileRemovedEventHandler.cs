using Domain.Features.People.DomainEvents.ProfileEvents;
using Domain.Shared.CustomProviders;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;
using UseCase.Shared.Interfaces;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.DomainEvents.PersonProfileRemoved
{
    public class PersonProfileRemovedEventHandler : INotificationHandler<PersonProfileRemovedEvent>
    {
        // Properties
        private static int _timeDaysValid = 30;
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IKafkaService _kafkaService;

        // Constructor
        public PersonProfileRemovedEventHandler(
            IKafkaService kafkaService,
            IAuthenticationGeneratorService authenticationGenerator)
        {
            _kafkaService = kafkaService;
            _authenticationGenerator = authenticationGenerator;
        }

        // Methods
        public async Task Handle(PersonProfileRemovedEvent notification, CancellationToken cancellationToken)
        {
            notification.UrlSegment = _authenticationGenerator.GenerateUrlSegment();
            notification.ValidTo = CustomTimeProvider.GetDateTime(
                CustomTimeProvider.Today.AddDays(_timeDaysValid));
            var log = (UserProfileRemovedMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
