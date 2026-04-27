using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.JsonConverters
{
    public class UIntFromStringConverter : JsonConverter<uint>
    {
        public override uint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetUInt32(),

                JsonTokenType.String => Parse(reader.GetString()),

                _ => throw new JsonException($"Unexpected token parsing uint. Token: {reader.TokenType}")
            };
        }

        private static uint Parse(string value)
        {
            if (uint.TryParse(value, out var result))
                return result;

            throw new JsonException($"Invalid uint value: '{value}'");
        }

        public override void Write(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
