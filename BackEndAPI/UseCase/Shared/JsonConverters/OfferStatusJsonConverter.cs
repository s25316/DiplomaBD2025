// Ignore Spelling: Json

using Domain.Shared.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;
using UseCase.Shared.DTOs.Responses.Companies.Offers;

namespace UseCase.Shared.JsonConverters
{
    public class OfferStatusJsonConverter : JsonConverter<OfferStatus>
    {
        public override OfferStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, OfferStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Description());
        }
    }
}
