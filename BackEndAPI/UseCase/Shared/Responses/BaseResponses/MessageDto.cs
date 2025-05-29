namespace UseCase.Shared.Responses.BaseResponses
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }

        public Guid ProcessId { get; set; }

        public DateTime Created { get; set; }

        //public DateTime? Removed { get; set; }

        public DateTime? Read { get; set; }

        public bool IsPersonSend { get; set; }

        public string? Message { get; set; }
    }
}
