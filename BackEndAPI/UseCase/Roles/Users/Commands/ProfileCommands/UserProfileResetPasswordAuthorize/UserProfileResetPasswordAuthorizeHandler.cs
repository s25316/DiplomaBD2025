using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordAuthorize.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Services.Authentication.Generators;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordAuthorize
{
    public class UserProfileResetPasswordAuthorizeHandler : IRequestHandler<UserProfileResetPasswordAuthorizeRequest, ProfileCommandResponse>
    {
        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGeneratorService;
        private readonly IAuthenticationInspectorService _authenticationInspectorService;
        private readonly IPersonRepository _personRepository;
        private readonly IMediator _mediator;


        // Constructor
        public UserProfileResetPasswordAuthorizeHandler(
            IAuthenticationGeneratorService authenticationGeneratorService,
            IAuthenticationInspectorService authenticationInspectorService,
            IPersonRepository personRepository,
            IMediator mediator)
        {
            _mediator = mediator;
            _personRepository = personRepository;
            _authenticationGeneratorService = authenticationGeneratorService;
            _authenticationInspectorService = authenticationInspectorService;
        }


        // Methods
        public async Task<ProfileCommandResponse> Handle(UserProfileResetPasswordAuthorizeRequest request, CancellationToken cancellationToken)
        {
            var userId = GetPersonId(request);
            var selectResult = await _personRepository.GetAsync(userId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareInvalid();
            }
            var domainPerson = selectResult.Item;

            // Check Password
            var hashedPassword = _authenticationGeneratorService.HashPassword(
                domainPerson.Salt,
                request.Command.OldPassword);
            if (hashedPassword != domainPerson.Password)
            {
                return PrepareInvalid();
            }

            // Update Password
            var (salt, password) = _authenticationGeneratorService.HashPassword(request.Command.NewPassword);
            domainPerson.SetAuthenticationData(salt, password);
            foreach (var @event in domainPerson.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
            await _personRepository.UpdateAsync(domainPerson, cancellationToken);
            return PrepareValid();
        }

        //Static Methods
        public static ProfileCommandResponse PrepareValid()
        {
            return ProfileCommandResponse.PrepareResponse(HttpCode.Ok);
        }

        public static ProfileCommandResponse PrepareInvalid()
        {
            return ProfileCommandResponse.PrepareResponse(HttpCode.BadRequest);
        }

        // Non Static Methods
        private PersonId GetPersonId(UserProfileResetPasswordAuthorizeRequest request)
        {
            return _authenticationInspectorService.GetPersonId(request.Metadata.Claims);
        }
    }
}
