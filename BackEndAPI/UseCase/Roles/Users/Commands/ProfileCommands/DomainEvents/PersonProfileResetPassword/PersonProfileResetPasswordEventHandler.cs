using Domain.Features.People.DomainEvents.ProfileEvents;
using MediatR;
using UseCase.Kafka;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents;

namespace UseCase.Roles.Users.Commands.ProfileCommands.DomainEvents.PersonProfileResetPassword
{
    public class PersonProfileResetPasswordEventHandler : INotificationHandler<PersonProfileResetPasswordEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonProfileResetPasswordEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonProfileResetPasswordEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserProfileUpdatedPasswordMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
