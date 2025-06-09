using Domain.Features.Companies.DomainEvents;
using MediatR;

namespace UseCase.Roles.Administrators.DomainEvents.CompanyBlocked
{
    public class CompanyBlockedEventHandler : INotificationHandler<CompanyBlockedEvent>
    {
        public async Task Handle(CompanyBlockedEvent notification, CancellationToken cancellationToken)
        {
            // Email sending
            Console.WriteLine(notification);
        }
    }
}
