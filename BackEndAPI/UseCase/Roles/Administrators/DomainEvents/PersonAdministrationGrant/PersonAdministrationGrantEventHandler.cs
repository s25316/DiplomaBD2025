using Domain.Features.People.DomainEvents.AdministrationEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Administrators.DomainEvents.PersonAdministrationGrant
{
    public class PersonAdministrationGrantEventHandler : INotificationHandler<PersonAdministrationGrantEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonAdministrationGrantEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonAdministrationGrantEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserProfileGrantAdminMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
