using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibraProgramming.Data.OpenApi.Core
{
    internal sealed class GeoCoordinateConverter : JsonConverter<float>
    {
        public GeoCoordinateConverter()
        {
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(float) == typeToConvert;
        }

        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonTokenType.String == reader.TokenType)
            {
                var str = reader.GetString();
                return false == String.IsNullOrEmpty(str) ? Convert.ToSingle(str, CultureInfo.InvariantCulture) : 0.0f;
            }

            if (JsonTokenType.Number == reader.TokenType)
            {
                var value = reader.GetSingle();
                return value;
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}