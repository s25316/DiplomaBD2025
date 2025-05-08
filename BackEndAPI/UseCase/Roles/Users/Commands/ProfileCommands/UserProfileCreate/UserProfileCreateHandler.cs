using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileCreate.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Services.Authentication.Generators;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileCreate
{
    public class UserProfileCreateHandler : IRequestHandler<UserProfileCreateRequest, ProfileCommandResponse>
    {
        // Properties
        public readonly IMediator _mediator;
        public readonly IPersonRepository _personRepository;
        public readonly IAuthenticationGeneratorService _authenticationGenerator;


        // Constructor
        public UserProfileCreateHandler(
            IMediator mediator,
            IPersonRepository personRepository,
            IAuthenticationGeneratorService authenticationGenerator)
        {
            _mediator = mediator;
            _personRepository = personRepository;
            _authenticationGenerator = authenticationGenerator;
        }


        // Methods
        public async Task<ProfileCommandResponse> Handle(UserProfileCreateRequest request, CancellationToken cancellationToken)
        {
            var personBuilder = PrepareBuilder(request);
            if (personBuilder.HasErrors())
            {
                return PrepareResponse(
                    HttpCode.BadRequest,
                    personBuilder.GetErrors());
            }

            var domainPerson = personBuilder.Build();
            var createResult = await _personRepository
                .CreateAsync(domainPerson, cancellationToken);

            if (createResult.Code != HttpCode.Created)
            {
                return PrepareResponse(
                    createResult.Code,
                    createResult.Metadata.Message);
            }

            foreach (var domainEvent in domainPerson.DomainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            return PrepareResponse(
                    createResult.Code,
                    createResult.Metadata.Message);
        }

        // Private Static Methods
        private static ProfileCommandResponse PrepareResponse(HttpCode code, string? message = null)
        {
            return ProfileCommandResponse.PrepareResponse(code, message);
        }

        // Private Non Static Methods
        private DomainPerson.Builder PrepareBuilder(UserProfileCreateRequest request)
        {
            var (salt, hashedPassword) = _authenticationGenerator
                .HashPassword(request.Command.Password);

            return new DomainPerson.Builder()
                .SetLogin(request.Command.Email)
                .SetAuthenticationData(
                    salt,
                    hashedPassword)
                .SetHasTwoFactorAuthentication(false)
                .SetIsAdministrator(false)
                .SetIsStudent(false);
        }
    }
}
