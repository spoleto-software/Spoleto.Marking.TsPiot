using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.Models
{
    public record CodesCheckResult
    {
        [JsonPropertyName("codesResponse")]
        public CodesResponse? CodesResponse { get; set; }

        [JsonPropertyName("error")]
        public Error? Error { get; set; }

        [JsonPropertyName("code")]
        public int? Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("details")]
        public List<string>? Details { get; set; }

        [JsonIgnore]
        public bool IsSuccess => CodesResponse is not null && Error is null;

        [JsonIgnore]
        public bool IsEmergencyMode => Code == 203;
    }
}
