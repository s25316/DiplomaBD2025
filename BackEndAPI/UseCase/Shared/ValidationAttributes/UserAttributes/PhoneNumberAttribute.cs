using Domain.Features.People.ValueObjects.PhoneNumbers;
using System.ComponentModel.DataAnnotations;
using UseCase.Shared.ValidationAttributes.BaseAttributes;

namespace UseCase.Shared.ValidationAttributes.UserAttributes
{
    //AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PhoneNumberAttribute : BaseStringAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var func = BuildIsValid(phoneNumber => new PhoneNumber(phoneNumber).Value);
            return func(value, validationContext);
        }
    }
}
