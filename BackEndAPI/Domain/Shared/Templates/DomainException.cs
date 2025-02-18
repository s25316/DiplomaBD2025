namespace Domain.Shared.Templates
{
    class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
