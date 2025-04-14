namespace Domain.Features.ContractConditions.ValueObjects.HoursPerTerms
{
    public record HoursPerTerm
    {
        // Property
        public int Value { get; private set; }


        // Constructor
        public HoursPerTerm(int value)
        {
            if (value < 1)
            {
                throw new HoursPerTermException(
                    $"{Messages.Record_HoursPerTerm_Invalid}: {value}");
            }
            Value = value;
        }

        // Methods
        public static implicit operator HoursPerTerm(int val)
        {
            return new HoursPerTerm(val);
        }

        public static implicit operator int(HoursPerTerm val)
        {
            return val.Value;
        }
    }
}
