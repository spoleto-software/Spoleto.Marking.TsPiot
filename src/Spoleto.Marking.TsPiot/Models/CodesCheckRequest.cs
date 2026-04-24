using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.Models
{
    public record CodesCheckRequest
    {
        /// <summary>
        /// Массив кодов маркировки (КМ) в Base64.
        /// </summary>
        /// <remarks>
        /// Если КМ содержит крипто-подпись, символ GS (ASCII 029) должен быть закодирован в Base64 без дополнительного экранирования.
        /// </remarks>
        [JsonPropertyName("codes")]
        public List<string> Codes { get; set; } = [];

        /// <summary>
        /// Информация о ПМСР (кассовом ПО).
        /// </summary>
        [JsonPropertyName("client_info")]
        public ClientInfo ClientInfo { get; set; }
    }
}
