﻿using MediatR;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetNotificationTypes.Request
{
    public class GetNotificationTypesRequest : IRequest<IEnumerable<NotificationTypeDto>>
    {
    }
}
