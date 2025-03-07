﻿using MediatR;
using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.GetSkillTypes.Request
{
    public class GetSkillTypesRequest : IRequest<IEnumerable<SkillTypeDto>>
    {
    }
}
