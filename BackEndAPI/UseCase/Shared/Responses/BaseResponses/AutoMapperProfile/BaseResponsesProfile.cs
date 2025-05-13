using AutoMapper;
using Domain.Features.Offers.Enums;
using Domain.Shared.CustomProviders;
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

            CreateMap<Offer, OfferDto>()
                .ConstructUsing((db, context) =>
                {
                    var now = CustomTimeProvider.Now;
                    OfferStatus status = OfferStatus.Active;
                    if (db.PublicationStart > now)
                    {
                        status = OfferStatus.Scheduled;
                    }
                    if (db.PublicationEnd.HasValue && db.PublicationEnd <= now)
                    {
                        status = OfferStatus.Expired;
                    }

                    return new OfferDto
                    {
                        OfferId = db.OfferId,
                        BranchId = db.BranchId,
                        PublicationStart = db.PublicationStart,
                        PublicationEnd = db.PublicationEnd,
                        EmploymentLength = db.EmploymentLength,
                        WebsiteUrl = db.WebsiteUrl,
                        StatusId = (int)status,
                        Status = status,
                    };
                });
        }
    }
}
