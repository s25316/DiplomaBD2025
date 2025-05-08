// Ignore Spelling: Mongo
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordUnAuthorize.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordUnAuthorize
{
    public class UserProfileResetPasswordUnAuthorizeHandler : IRequestHandler<UserProfileResetPasswordUnAuthorizeRequest, ProfileCommandResponse>
    {
        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IPersonRepository _personRepository;
        private readonly IMongoDbService _mongoService;
        private readonly IMediator _mediator;


        // Constructor
        public UserProfileResetPasswordUnAuthorizeHandler(
            IAuthenticationGeneratorService authenticationGeneratorService,
            IPersonRepository personRepository,
            IMongoDbService mongoService,
            IMediator mediator)
        {
            _mediator = mediator;
            _mongoService = mongoService;
            _personRepository = personRepository;
            _authenticationGenerator = authenticationGeneratorService;
        }


        // Methods
        public async Task<ProfileCommandResponse> Handle(UserProfileResetPasswordUnAuthorizeRequest request, CancellationToken cancellationToken)
        {
            var resetPasswordInitiationDto = await _mongoService.GeUserResetPasswordInitiationAsync(
                request.UrlSegment1,
                request.UrlSegment2,
                cancellationToken);
            if (resetPasswordInitiationDto.Initiation == null ||
                resetPasswordInitiationDto.IsDeactivated ||
                !resetPasswordInitiationDto.IsValid)
            {
                return PrepareInvalid();
            }


            var selectResult = await _personRepository.GetAsync(
                resetPasswordInitiationDto.Initiation.UserId,
                cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareInvalid();
            }
            var domainPerson = selectResult.Item;


            var (salt, password) = _authenticationGenerator
                .HashPassword(request.Command.NewPassword);
            domainPerson.SetAuthenticationData(salt, password);
            foreach (var @event in domainPerson.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
            var updateResult = await _personRepository.UpdateAsync(domainPerson, cancellationToken);
            if (updateResult.Code != HttpCode.Ok)
            {
                throw new UseCaseLayerException(updateResult.Metadata.Message);
            }

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
    }
}
