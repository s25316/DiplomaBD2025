using AutoMapper;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.Responses.BaseResponses.AutoMapperProfile
{
    public class BaseResponsesProfile : Profile
    {
        public BaseResponsesProfile()
        {
            CreateMap<Company, CompanyDto>();
            CreateMap<OfferSkill, OfferSkillDto>();

            CreateMap<Address, AddressResponseDto>()
                .ConstructUsing(db => new AddressResponseDto
                {
                    CountryId = db.City.State.Country.CountryId,
                    CountryName = db.City.State.Country.Name,
                    StateId = db.City.State.StateId,
                    StateName = db.City.State.Name,
                    CityId = db.City.CityId,
                    CityName = db.City.Name,
                    StreetId = db.Street == null ? null : db.Street.StreetId,
                    StreetName = db.Street == null ? null : db.Street.Name,
                    HouseNumber = db.HouseNumber,
                    ApartmentNumber = db.ApartmentNumber,
                    PostCode = db.PostCode,
                    Lon = db.Lon,
                    Lat = db.Lat,
                });
        }
    }
}
