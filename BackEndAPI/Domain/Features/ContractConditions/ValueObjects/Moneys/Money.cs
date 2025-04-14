namespace Domain.Features.ContractConditions.ValueObjects.Moneys
{
    public record Money
    {
        // Property
        public decimal Value { get; init; }


        // Constructor
        public Money(decimal value)
        {
            if (!IsValidMoney(value))
            {
                throw new MoneyException($"{Messages.Record_Money_Invalid}: {value}");
            }
            Value = value;
        }

        // Methods
        public static implicit operator Money(decimal value)
        {
            return new Money(value);
        }

        public static implicit operator decimal(Money value)
        {
            return value.Value;
        }

        private static bool IsValidMoney(decimal value)
        {
            if (decimal.Round(value, 2) != value || value < 0)
            {
                return false;
            }
            return true;
        }
    }
}
