using Domain.Shared.Enums;
using Domain.Shared.ValueObjects.Emails;
using MediatR;
using UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserInitiateUpdatePersonPassword.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserInitiateUpdatePersonPassword
{
    public class UserInitiateUpdatePersonPasswordHandler : IRequestHandler<UserInitiateUpdatePersonPasswordRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IPersonRepository _personRepository;
        private readonly IMediator _mediator;


        // Constructor
        public UserInitiateUpdatePersonPasswordHandler(
            IPersonRepository personRepository,
            IMediator mediator)
        {
            _mediator = mediator;
            _personRepository = personRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserInitiateUpdatePersonPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Email login = (Email?)request.Command.Login
                    ?? throw new EmailException("Impossible, Declarative Validation in DTO");

                var selectResult = await _personRepository.GetAsync(login, cancellationToken);
                if (selectResult.Code != HttpCode.Ok)
                {
                    return PrepareValid();
                }

                var domainPerson = selectResult.Item;
                domainPerson.RaiseInitiateResetPasswordEvent();
                foreach (var domainEvent in domainPerson.DomainEvents)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
                domainPerson.ClearEvents();
                return PrepareValid();
            }
            catch (EmailException)
            {
                return PrepareInvalid();
            }
        }

        // Static Methods
        public static ResultMetadataResponse PrepareValid()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.Ok);
        }

        public static ResultMetadataResponse PrepareInvalid()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.BadRequest);
        }
        // Non Static Methods
    }
}
