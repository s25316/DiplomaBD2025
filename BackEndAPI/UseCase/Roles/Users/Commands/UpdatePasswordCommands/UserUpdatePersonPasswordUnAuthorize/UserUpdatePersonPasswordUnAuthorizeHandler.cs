﻿// Ignore Spelling: Mongo
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordUnAuthorize.Request;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.People;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordUnAuthorize
{
    public class UserUpdatePersonPasswordUnAuthorizeHandler : IRequestHandler<UserUpdatePersonPasswordUnAuthorizeRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IPersonRepository _personRepository;
        private readonly IMongoDbService _mongoService;


        // Constructor
        public UserUpdatePersonPasswordUnAuthorizeHandler(
            IAuthenticationGeneratorService authenticationGeneratorService,
            IPersonRepository personRepository,
            IMongoDbService mongoService)
        {
            _mongoService = mongoService;
            _personRepository = personRepository;
            _authenticationGenerator = authenticationGeneratorService;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserUpdatePersonPasswordUnAuthorizeRequest request, CancellationToken cancellationToken)
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


            var (salt, password) = _authenticationGenerator.HashPassword(request.Command.NewPassword);
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
    }
}
