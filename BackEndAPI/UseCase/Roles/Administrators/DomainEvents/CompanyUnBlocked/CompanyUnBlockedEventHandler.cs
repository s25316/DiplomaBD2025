using Domain.Features.Companies.DomainEvents;
using MediatR;

namespace UseCase.Roles.Administrators.DomainEvents.CompanyUnBlocked
{
    public class CompanyUnBlockedEventHandler : INotificationHandler<CompanyUnBlockedEvent>
    {
        public async Task Handle(CompanyUnBlockedEvent notification, CancellationToken cancellationToken)
        {
            // Email sending
            Console.WriteLine(notification);
        }
    }
}
