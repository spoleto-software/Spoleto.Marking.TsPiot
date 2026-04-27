using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.Models
{
    public record LmInfo
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("lastSync")]
        public string LastSync { get; set; }

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
