﻿// Ignore Spelling: Dto

using UseCase.Shared.ValidationAttributes.PaginationAttributes;

namespace UseCase.Shared.Requests.QueryParameters
{
    public sealed class PaginationQueryParametersDto
    {
        [Page]
        public int Page { get; init; }


        [ItemsPerPage]
        public int ItemsPerPage { get; init; }
    }
}
