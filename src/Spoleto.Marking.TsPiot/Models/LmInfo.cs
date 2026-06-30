using System.Text.Json.Serialization;
using Spoleto.Marking.TsPiot.JsonConverters;

namespace Spoleto.Marking.TsPiot.Models
{
    public record LmInfo
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("lastSync")]
        [JsonConverter(typeof(FlexibleLongConverter))]
        public long LastSyncMilliseconds { get; set; }

        [JsonIgnore]
        public DateTime LastSync => DateTimeOffset.FromUnixTimeMilliseconds(LastSyncMilliseconds).DateTime;

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("expDate")]
        public DateTime ExpDate { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("pass")]
        public string Pass { get; set; }
    }
}
