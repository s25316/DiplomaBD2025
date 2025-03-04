namespace Domain.Features.Offers.ValueObjects
{
    public record SalaryRange
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }

        public SalaryRange(decimal min, decimal max)
        {
            if (min > max)
            {
                Min = max;
                Max = min;
            }
            else
            {
                Min = min;
                Max = max;
            }
        }
    }
}
