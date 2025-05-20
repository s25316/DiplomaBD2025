using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.RegistrationCommands.UserCreatePerson.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Generators;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.RegistrationCommands.UserCreatePerson
{
    public class UserCreatePersonHandler : IRequestHandler<UserCreatePersonRequest, ResultMetadataResponse>
    {
        // Properties
        public readonly IPersonRepository _personRepository;
        public readonly IAuthenticationGeneratorService _authenticationGenerator;


        // Constructor
        public UserCreatePersonHandler(
            IPersonRepository personRepository,
            IAuthenticationGeneratorService authenticationGenerator)
        {
            _personRepository = personRepository;
            _authenticationGenerator = authenticationGenerator;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserCreatePersonRequest request, CancellationToken cancellationToken)
        {
            var personBuilder = PrepareBuilder(request);
            if (personBuilder.HasErrors())
            {
                return PrepareResponse(
                    HttpCode.BadRequest,
                    personBuilder.GetErrors());
            }

            var domainPerson = personBuilder.Build();
            var createResult = await _personRepository.CreateAsync(
                domainPerson,
                cancellationToken);

            return PrepareResponse(
                createResult.Code,
                createResult.Metadata.Message);
        }

        // Private Static Methods
        private static ResultMetadataResponse PrepareResponse(HttpCode code, string? message = null)
        {
            return ResultMetadataResponse.PrepareResponse(code, message);
        }

        // Private Non Static Methods
        private DomainPerson.Builder PrepareBuilder(UserCreatePersonRequest request)
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
