namespace UseCase.Shared.Services
{
    public interface IEmailService
    {
        Task SendAsync(
           string email,
           string title,
           string message,
           CancellationToken cancellationToken = default);
    }
}
