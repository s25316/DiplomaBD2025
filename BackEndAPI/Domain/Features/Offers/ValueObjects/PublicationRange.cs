namespace Domain.Features.Offers.ValueObjects
{
    public record PublicationRange
    {
        // Properties
        public DateTime Start { get; private set; }
        public DateTime? End { get; private set; }


        //Constructor
        public PublicationRange(DateTime start, DateTime? end)
        {
            if (end.HasValue && end < start)
            {
                Start = end.Value;
                End = start;
            }
            else
            {
                Start = start;
                End = end;
            }
        }
    }
}
