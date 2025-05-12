using AutoMapper;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.Responses.BaseResponses.CompanyUser.AutoMapperProfile
{
    public class CompanyUserProfile : Profile
    {
        public CompanyUserProfile()
        {
            CreateMap<Branch, CompanyUserBranchDto>();
        }
    }
}
