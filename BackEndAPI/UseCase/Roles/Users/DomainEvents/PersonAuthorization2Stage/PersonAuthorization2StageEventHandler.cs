﻿using Domain.Features.People.DomainEvents.AuthorizationEvents;
using MediatR;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Users.DomainEvents.PersonAuthorization2Stage
{
    public class PersonAuthorization2StageEventHandler : INotificationHandler<PersonAuthorization2StageEvent>
    {
        // Properties
        private readonly IKafkaService _kafkaService;


        // Constructor
        public PersonAuthorization2StageEventHandler(
            IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }


        // Methods
        public async Task Handle(PersonAuthorization2StageEvent notification, CancellationToken cancellationToken)
        {
            var log = (UserAuthorization2StageMongoDb)notification;
            await _kafkaService.SendUserLogAsync(log, cancellationToken);
        }
    }
}
