using Domain.Shared.ValueObjects.Emails;
using MediatR;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordInitiate.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordInitiate.Response;
using UseCase.Roles.Users.Repositories;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordInitiate
{
    public class UserProfileResetPasswordInitiateHandler : IRequestHandler<UserProfileResetPasswordInitiateRequest, UserProfileResetPasswordInitiateResponse>
    {
        // Properties
        private readonly IPersonRepository _personRepository;
        private readonly IMediator _mediator;


        // Constructor
        public UserProfileResetPasswordInitiateHandler(
            IPersonRepository personRepository,
            IMediator mediator)
        {
            _mediator = mediator;
            _personRepository = personRepository;
        }


        // Methods
        public async Task<UserProfileResetPasswordInitiateResponse> Handle(UserProfileResetPasswordInitiateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Email login = (Email?)request.Command.Login
                    ?? throw new EmailException("Impossible, Declarative Validation in DTO");

                var selectResult = await _personRepository.GetAsync(login, cancellationToken);
                if (selectResult.Code != Domain.Shared.Enums.HttpCode.Ok)
                {
                    return PrepareValid();
                }

                var domainPerson = selectResult.Item;
                domainPerson.RaiseInitiateResetPasswordEvent();
                foreach (var domainEvent in domainPerson.DomainEvents)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
                return PrepareValid();
            }
            catch (EmailException)
            {
                return PrepareInvalid();
            }
        }

        // Static Methods
        public static UserProfileResetPasswordInitiateResponse PrepareValid()
        {
            return UserProfileResetPasswordInitiateResponse.PrepareValid();
        }

        public static UserProfileResetPasswordInitiateResponse PrepareInvalid()
        {
            return UserProfileResetPasswordInitiateResponse.PrepareInvalid();
        }
        // Non Static Methods
    }
}
