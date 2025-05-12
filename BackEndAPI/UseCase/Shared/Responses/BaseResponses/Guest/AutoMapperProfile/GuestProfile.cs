using AutoMapper;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;

namespace UseCase.Shared.Responses.BaseResponses.Guest.AutoMapperProfile
{
    public class GuestProfile : Profile
    {
        public GuestProfile()
        {
            CreateMap<Branch, GuestBranchDto>();

        }
    }
}
