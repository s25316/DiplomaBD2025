-- DROP PROCEDURE Address_CREATE;
-- OUR OWN IMPLEMENTATION OF PROCEDURES
--======================================================================================
--======================================================================================
--[Address CREATE]
CREATE OR ALTER PROCEDURE Address_CREATE
	@CountryName nvarchar(100),
	@StateName nvarchar(100),
	@CityName nvarchar(100),
	@StreetName nvarchar(100) NULL,
	@HouseNumber nvarchar(25),
	@ApartmentNumber nvarchar(25) NULL,
	@PostCode nvarchar(25),
	@Longitude real,
	@Latitude real,
	@AddressId uniqueidentifier OUTPUT 
AS
BEGIN
	IF NOT EXISTS (
		SELECT *
		FROM [dbo].[Addresses]
		WHERE Lon = @Longitude AND Lat = @Latitude
	)
		BEGIN 
			DECLARE @CountryId int;
			DECLARE @StateId int;
			DECLARE @CityId int;
			DECLARE @StreetId int;

			--COUNTRY
			IF NOT EXISTS (
				SELECT * 
				FROM [dbo].[Countries]
				WHERE Name LIKE @CountryName
			)
				BEGIN
					INSERT INTO [dbo].[Countries]  
					(Name)
					VALUES
					(@CountryName);
				END;
			SELECT TOP 1 @CountryId = CountryId
			FROM [dbo].[Countries]
			WHERE Name LIKE @CountryName;

			--STATE
			IF NOT EXISTS (
				SELECT * 
				FROM [dbo].[States]
				WHERE Name LIKE @StateName AND CountryId = @CountryId
			)
				BEGIN
					INSERT INTO [dbo].[States]
					(Name, CountryId)
					VALUES
					(@StateName, @CountryId);
				END;
			SELECT TOP 1 @StateId = StateId
			FROM [dbo].[States]
			WHERE Name LIKE @StateName AND CountryId = @CountryId;

			--CITY
			IF NOT EXISTS (
				SELECT * 
				FROM [dbo].[Cities]
				WHERE Name LIKE @CityName AND StateId = @StateId
			)
				BEGIN
					INSERT INTO [dbo].[Cities]
					(Name, StateId)
					VALUES
					(@CityName, @StateId);
				END;
			SELECT TOP 1 @CityId = CityId
			FROM [dbo].[Cities]
			WHERE Name LIKE @CityName AND StateId = @StateId;

			--STREET
			IF @StreetName IS NOT NULL
				BEGIN
					IF NOT EXISTS (
						SELECT * 
						FROM [dbo].[Streets]
						WHERE Name LIKE @StreetName 
					)
						BEGIN
							INSERT INTO [dbo].[Streets]
							(Name)
							VALUES
							(@StreetName);
						END;
					SELECT TOP 1 @StreetId = StreetId
					FROM [dbo].[Streets]
					WHERE Name LIKE @StreetName;
				END;

			INSERT INTO [dbo].[Addresses]
			(CityId, StreetId, HouseNumber, ApartmentNumber, PostCode, Lon, Lat, Point)
			VALUES
			(@CityId, @StreetId, @HouseNumber, @ApartmentNumber, @PostCode, @Longitude, @Latitude,
			GEOGRAPHY::Point(@Latitude, @Longitude, 4326));
		END;
	SELECT @AddressId = AddressId
	FROM [dbo].[Addresses]
	WHERE Lon = @Longitude AND Lat = @Latitude;
END;