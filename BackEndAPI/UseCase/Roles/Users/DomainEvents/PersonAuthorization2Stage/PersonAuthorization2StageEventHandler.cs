using Domain.Features.People.DomainEvents.AuthorizationEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.Shared.Interfaces;
using UseCase.Shared.Services;

namespace UseCase.Roles.Users.DomainEvents.PersonAuthorization2Stage
{
    public class PersonAuthorization2StageEventHandler : INotificationHandler<PersonAuthorization2StageEvent>
    {
        private const string TITTLE = "[Code For Login In]";
        private const string TEXT = "For login in put this code:";

        // Properties
        private readonly IKafkaService _kafkaService;
        private readonly IEmailService _emailService;


        // Constructor
        public PersonAuthorization2StageEventHandler(
            IKafkaService kafkaService,
            IEmailService emailService)
        {
            _emailService = emailService;
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonAuthorization2StageEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserAuthorization2StageMongoDb)notification;

            await _emailService.SendAsync(
                notification.Email,
                TITTLE,
                $"{TEXT} {notification.Code}",
                cancellationToken);

            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
