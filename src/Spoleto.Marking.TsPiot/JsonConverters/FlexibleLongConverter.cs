using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.JsonConverters
{
    public class FlexibleLongConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetInt64(),

                JsonTokenType.String => Parse(reader.GetString()),

                _ => throw new JsonException($"Unexpected token parsing long. Token: {reader.TokenType}")
            };
        }

        private static long Parse(string value)
        {
            if (long.TryParse(value, out var result))
                return result;

            throw new JsonException($"Invalid long value: '{value}'");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
