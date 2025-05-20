using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordAuthorize.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Generators;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordAuthorize
{
    public class UserUpdatePersonPasswordAuthorizeHandler : IRequestHandler<UserUpdatePersonPasswordAuthorizeRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGeneratorService;
        private readonly IAuthenticationInspectorService _authenticationInspectorService;
        private readonly IPersonRepository _personRepository;


        // Constructor
        public UserUpdatePersonPasswordAuthorizeHandler(
            IAuthenticationGeneratorService authenticationGeneratorService,
            IAuthenticationInspectorService authenticationInspectorService,
            IPersonRepository personRepository)
        {
            _personRepository = personRepository;
            _authenticationGeneratorService = authenticationGeneratorService;
            _authenticationInspectorService = authenticationInspectorService;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserUpdatePersonPasswordAuthorizeRequest request, CancellationToken cancellationToken)
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
            var updateResult = await _personRepository.UpdateAsync(domainPerson, cancellationToken);
            if (updateResult.Code != HttpCode.Ok)
            {
                throw new UseCaseLayerException(updateResult.Metadata.Message);
            }

            return PrepareValid();
        }

        //Static Methods
        public static ResultMetadataResponse PrepareValid()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.Ok);
        }

        public static ResultMetadataResponse PrepareInvalid()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.BadRequest);
        }

        // Non Static Methods
        private PersonId GetPersonId(UserUpdatePersonPasswordAuthorizeRequest request)
        {
            return _authenticationInspectorService.GetPersonId(request.Metadata.Claims);
        }
    }
}
