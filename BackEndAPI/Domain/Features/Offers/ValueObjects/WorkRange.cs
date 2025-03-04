namespace Domain.Features.Offers.ValueObjects
{
    public record WorkRange
    {
        // Properties
        public DateOnly Start { get; private set; }
        public DateOnly? End { get; private set; }


        //Constructor
        public WorkRange(DateOnly start, DateOnly? end)
        {
            if (end.HasValue && end < start)
            {
                End = start;
                Start = end.Value;
            }
            else
            {
                Start = start;
                End = end;
            }
        }
    }
}
