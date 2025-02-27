using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.People.Exceptions
{
    public class PersonException : DomainException
    {
        // Constructor
        public PersonException(string? message, HttpCodeEnum code) : base(message, code)
        {
        }
    }
}
