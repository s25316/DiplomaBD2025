namespace Domain.Features.Offers.ValueObjects.EmploymentLengths
{
    public record EmploymentLength
    {
        // Property
        public float Value { get; init; }


        // Constructor
        public EmploymentLength(float value)
        {
            if (value < 0)
            {
                throw new EmploymentLengthException(
                    $"{Messages.Record_EmploymentLength_Invalid}: {value}");
            }
            Value = value;
        }


        // Methods
        public static implicit operator EmploymentLength(float value)
        {
            return new EmploymentLength(value);
        }

        public static implicit operator float(EmploymentLength value)
        {
            return value.Value;
        }
    }
}
