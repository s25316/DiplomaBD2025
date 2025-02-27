using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UseCase.RelationalDatabase;
using UseCase.Shared.DTOs.Requests;
using UseCase.Shared.Exceptions;

namespace UseCase.Shared.Repositories.Addresses
{
    public class AddressRepository : IAddressRepository
    {
        // Properties 
        private readonly DiplomaBdContext _context;

        // Constructor
        public AddressRepository(DiplomaBdContext context)
        {
            _context = context;
        }


        // Public Methods
        public async Task<Guid> CreateAddressAsync(AddressRequestDto request)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@AddressId",
                SqlDbType = SqlDbType.UniqueIdentifier,
                Direction = ParameterDirection.Output
            };
            await _context.Database
                .ExecuteSqlRawAsync(
                @"EXEC Address_CREATE 
                        @CountryName, @StateName, @CityName, @StreetName, @HouseNumber, 
                        @ApartmentNumber, @PostCode, @Longitude, @Latitude, 
                        @AddressId OUTPUT",
                new SqlParameter("@CountryName", request.CountryName),
                new SqlParameter("@StateName", request.StateName),
                new SqlParameter("@CityName", request.CityName),
                new SqlParameter("@StreetName", (
                    string.IsNullOrWhiteSpace(request.StreetName) ?
                    (object)DBNull.Value :
                    request.StreetName
                )),
                new SqlParameter("@HouseNumber", request.HouseNumber),
                new SqlParameter("@ApartmentNumber", (
                    string.IsNullOrWhiteSpace(request.ApartmentNumber) ?
                    (object)DBNull.Value :
                    request.ApartmentNumber
                )),
                new SqlParameter("@PostCode", request.PostCode),
                new SqlParameter("@Longitude", request.Lon),
                new SqlParameter("@Latitude", request.Lat),
                idParam);
            var id = idParam.Value.ToString() ?? throw new UseCaseLayerException(
                Messages.Procedure_Address_Create_InvalidResult
                );
            return Guid.Parse(id);
        }
    }
}
