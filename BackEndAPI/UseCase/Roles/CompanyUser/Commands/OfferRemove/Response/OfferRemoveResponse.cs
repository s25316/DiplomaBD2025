﻿using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.OfferRemove.Response
{
    public class OfferRemoveResponse : ResponseTemplate<ResponseCommandMetadata>
    {
        public static OfferRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new OfferRemoveResponse
            {
                HttpCode = code,
                Result = new ResponseCommandMetadata
                {
                    IsCorrect = intCode >= 200 && intCode < 300,
                    Message = message ?? code.Description()
                }
            };
        }
    }
}
