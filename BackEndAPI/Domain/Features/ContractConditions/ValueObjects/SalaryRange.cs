using Domain.Features.ContractConditions.Exceptions;

namespace Domain.Features.ContractConditions.ValueObjects
{
    public record SalaryRange
    {
        public Money Min { get; set; }
        public Money Max { get; set; }

        public SalaryRange(decimal min, decimal max)
        {
            if (min < 0 || max < 0)
            {
                throw new SalaryRangeException(Messages.Record_SalaryRange_Invalid);
            }

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
