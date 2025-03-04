using Domain.Shared.CustomProviders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.Users.Commands.UserRegistration.Request;
using UseCase.Roles.Users.Commands.UserRegistration.Response;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.Commands.UserRegistration
{
    public class UserRegistrationHandler : IRequestHandler<UserRegistrationRequest, UserRegistrationResponse>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationGeneratorService _authenticationGenerator;


        // Constructor
        public UserRegistrationHandler(
            DiplomaBdContext context,
            IAuthenticationGeneratorService authenticationGenerator)
        {
            _context = context;
            _authenticationGenerator = authenticationGenerator;
        }

        public async Task<UserRegistrationResponse> Handle(UserRegistrationRequest request, CancellationToken cancellationToken)
        {
            var existPerson = await _context.People
                .Where(person => person.Login == request.Command.Login)
                .FirstOrDefaultAsync(cancellationToken);

            if (existPerson != null)
            {
                return new UserRegistrationResponse
                {
                    IsCreated = false,
                    Message = "Choose other Login",
                };
            }
            var hashData = _authenticationGenerator
                .HashPassword(request.Command.Password);

            var person = new Person
            {
                Login = request.Command.Login,
                Salt = hashData.Salt,
                Password = hashData.HashedPassword,
                Created = CustomTimeProvider.GetDateTimeNow(),

            };
            await _context.People.AddAsync(person, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserRegistrationResponse
            {
                IsCreated = true,
                Message = "Created"
            };
        }
    }
}
