using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Commands.BranchCreate.Request;
using UseCase.Roles.CompanyUser.Commands.BranchCreate.Response;
using UseCase.Shared.DTOs.Requests;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Services.Time;

namespace UseCase.Roles.CompanyUser.Commands.BranchCreate
{
    public class BranchCreateHandler : IRequestHandler<BranchCreateRequest, BranchCreateResponse>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly ITimeService _timeService;
        private readonly IAuthenticationInspectorService _authenticationInspector;

        // Constructor
        public BranchCreateHandler(
            DiplomaBdContext context,
            ITimeService timeService,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _timeService = timeService;
            _authenticationInspector = authenticationInspector;
        }

        // Methods
        public async Task<BranchCreateResponse> Handle(BranchCreateRequest request, CancellationToken cancellationToken)
        {
            var now = _timeService.GetNow();
            var userId = _authenticationInspector.GetClaimsName(request.Metadata.Claims);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new BranchCreateResponse();
            }
            var userIdGuid = Guid.Parse(userId);
            var companyRolesCount = await _context.CompanyPeople
                .Where(conn =>
                    conn.PersonId == userIdGuid &&
                    conn.CompanyId == request.CompanyId)
                .CountAsync(cancellationToken);
            if (companyRolesCount == 0)
            {
                return new BranchCreateResponse();
            }

            var addrId = await CreateAddreessAsync(request.Command.Address, cancellationToken);



            return new BranchCreateResponse
            {
                BranchId = addrId,
            };
        }


        public async Task<Guid> CreateAddreessAsync(AddressRequestDto request, CancellationToken cancellationToken)
        {
            var country = await _context.Countries
                .Where(country => country.Name == request.CountryName)
                .FirstOrDefaultAsync(cancellationToken);
            var street = await _context.Streets
                .Where(street => street.Name == request.StreetName)
                .FirstOrDefaultAsync(cancellationToken);
            if (country == null)
            {
                var c = new Country
                {
                    Name = request.CountryName,
                };
                var s = new State
                {
                    Name = request.StateName,
                    Country = c,
                };
                var ci = new City
                {
                    Name = request.CityName,
                    State = s,
                };
                var addr = new Address
                {
                    City = ci,
                    HouseNumber = request.HouseNumber,
                    ApartmentNumber = request.ApartmentNumber,
                    PostCode = request.PostCode,
                    Lon = request.Lon,
                    Lat = request.Lat,
                    Point = new Point(21.0122, 52.2297) { SRID = 4326 },
                };


                if (street == null)
                {
                    var str = new Street
                    {
                        Name = request.StateName,
                    };
                    addr.Street = str;

                    await _context.Countries.AddAsync(c, cancellationToken);
                    await _context.States.AddAsync(s, cancellationToken);
                    await _context.Cities.AddAsync(ci, cancellationToken);
                    await _context.Addresses.AddAsync(addr, cancellationToken);
                    await _context.Streets.AddAsync(str, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return addr.AddressId;
                }
                else
                {
                    addr.Street = street;
                    await _context.Countries.AddAsync(c, cancellationToken);
                    await _context.States.AddAsync(s, cancellationToken);
                    await _context.Cities.AddAsync(ci, cancellationToken);
                    await _context.Addresses.AddAsync(addr, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return addr.AddressId;
                }

            }

            var state = await _context.States
                .Where(street => street.Name == request.StateName)
                .FirstOrDefaultAsync(cancellationToken);

            if (state == null)
            {
                var s = new State
                {
                    Name = request.StateName,
                    Country = country,
                };
                var ci = new City
                {
                    Name = request.CityName,
                    State = s,
                };
                var addr = new Address
                {
                    City = ci,
                    HouseNumber = request.HouseNumber,
                    ApartmentNumber = request.ApartmentNumber,
                    PostCode = request.PostCode,
                    Lon = request.Lon,
                    Lat = request.Lat,
                    Point = new Point(21.0122, 52.2297) { SRID = 4326 },
                };


                if (street == null)
                {
                    var str = new Street
                    {
                        Name = request.StateName,
                    };
                    addr.Street = str;

                    await _context.States.AddAsync(s, cancellationToken);
                    await _context.Cities.AddAsync(ci, cancellationToken);
                    await _context.Addresses.AddAsync(addr, cancellationToken);
                    await _context.Streets.AddAsync(str, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return addr.AddressId;
                }
                else
                {
                    addr.Street = street;
                    await _context.States.AddAsync(s, cancellationToken);
                    await _context.Cities.AddAsync(ci, cancellationToken);
                    await _context.Addresses.AddAsync(addr, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return addr.AddressId;
                }
            }


            var city = await _context.Cities
                .Where(street => street.Name == request.CityName)
                .FirstOrDefaultAsync(cancellationToken);
            if (city == null)
            {
                var ci = new City
                {
                    Name = request.CityName,
                    State = state,
                };
                var addr = new Address
                {
                    City = ci,
                    HouseNumber = request.HouseNumber,
                    ApartmentNumber = request.ApartmentNumber,
                    PostCode = request.PostCode,
                    Lon = request.Lon,
                    Lat = request.Lat,
                    Point = new Point(21.0122, 52.2297) { SRID = 4326 },
                };


                if (street == null)
                {
                    var str = new Street
                    {
                        Name = request.StateName,
                    };
                    addr.Street = str;

                    await _context.Cities.AddAsync(ci, cancellationToken);
                    await _context.Addresses.AddAsync(addr, cancellationToken);
                    await _context.Streets.AddAsync(str, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return addr.AddressId;
                }
                else
                {
                    addr.Street = street;
                    await _context.Cities.AddAsync(ci, cancellationToken);
                    await _context.Addresses.AddAsync(addr, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return addr.AddressId;
                }
            }

            var address = new Address
            {
                City = city,
                HouseNumber = request.HouseNumber,
                ApartmentNumber = request.ApartmentNumber,
                PostCode = request.PostCode,
                Lon = request.Lon,
                Lat = request.Lat,
                Point = new Point(21.0122, 52.2297) { SRID = 4326 },
            };


            if (street == null)
            {
                var str = new Street
                {
                    Name = request.StateName,
                };
                address.Street = str;

                await _context.Addresses.AddAsync(address, cancellationToken);
                await _context.Streets.AddAsync(str, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return address.AddressId;
            }
            else
            {
                address.Street = street;
                await _context.Addresses.AddAsync(address, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return address.AddressId;
            }
        }
    }
}
