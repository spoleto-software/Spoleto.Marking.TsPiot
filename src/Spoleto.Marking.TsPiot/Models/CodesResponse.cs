using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.Models
{
    public record CodesResponse
    {
        /// <summary>
        /// Результаты проверки кодов маркировки.
        /// </summary>
        [JsonPropertyName("codesResponse")]
        public List<CodeResponse> CodeResponses { get; set; }
    }
}
