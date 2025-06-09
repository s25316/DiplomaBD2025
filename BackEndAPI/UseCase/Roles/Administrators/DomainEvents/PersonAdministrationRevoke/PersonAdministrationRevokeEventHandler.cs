using Domain.Features.People.DomainEvents.AdministrationEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Administrators.DomainEvents.PersonAdministrationRevoke
{
    public class PersonAdministrationRevokeEventHandler : INotificationHandler<PersonAdministrationRevokeEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonAdministrationRevokeEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonAdministrationRevokeEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserProfileRevokeAdminMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
