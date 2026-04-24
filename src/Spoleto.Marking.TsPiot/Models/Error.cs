using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.Models
{
    public record Error
    {
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("errorDescription")]
        public string? ErrorDescription { get; set; }
    }
}
