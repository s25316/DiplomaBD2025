using Domain.Shared.CustomProviders;

namespace Domain.Features.People.ValueObjects.BirthDates
{
    public record BirthDate
    {
        // Properties
        public DateOnly Value { get; private set; }


        // Constructor
        public BirthDate(DateOnly value)
        {
            var today = CustomTimeProvider.Today;
            if (value > today)
            {
                throw new BirthDateException(Messages.Record_BirthDate_Future);
            }
            Value = value;
        }


        // Methods
        public static implicit operator BirthDate?(DateOnly? value)
        {
            return value.HasValue
                ? new BirthDate(value.Value)
                : null;
        }

        public static implicit operator DateOnly?(BirthDate? value)
        {
            return value?.Value;
        }

        public static implicit operator BirthDate(DateOnly value)
        {
            return new BirthDate(value);
        }

        public static implicit operator DateOnly(BirthDate value)
        {
            return value.Value;
        }
    }
}
