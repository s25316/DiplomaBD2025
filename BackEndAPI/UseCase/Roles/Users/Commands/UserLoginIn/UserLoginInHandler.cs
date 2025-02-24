using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Users.Commands.UserLoginIn.Request;
using UseCase.Roles.Users.Commands.UserLoginIn.Response;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.Commands.UserLoginIn
{
    public class UserLoginInHandler : IRequestHandler<UserLoginInRequest, UserLoginInResponse>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationGeneratorService _authenticationGenerator;


        // Constructor
        public UserLoginInHandler(
            DiplomaBdContext context,
            IAuthenticationGeneratorService authenticationGenerator)
        {
            _context = context;
            _authenticationGenerator = authenticationGenerator;
        }

        public async Task<UserLoginInResponse> Handle(UserLoginInRequest request, CancellationToken cancellationToken)
        {
            var databsePerson = await _context.People
                .Where(person => person.Login == request.Command.Login)
                .FirstOrDefaultAsync(cancellationToken);
            if (databsePerson == null)
            {
                return new UserLoginInResponse
                {
                    IsSuccess = false,
                };
            }
            var inputPassword = _authenticationGenerator.HashPassword(
                databsePerson.Salt,
                request.Command.Password);

            if (databsePerson.Password != inputPassword)
            {
                return new UserLoginInResponse
                {
                    IsSuccess = false,
                };
            }

            var token = _authenticationGenerator.GenerateJwt(
                databsePerson.PersonId.ToString(),
                ["Person"]
                );
            return new UserLoginInResponse
            {
                IsSuccess = true,
                Token = token.Jwt,
                ValidTo = token.ValidTo,
            };

        }
    }
}
