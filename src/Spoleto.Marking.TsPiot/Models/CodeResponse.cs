using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.Models
{
    public record CodeResponse
    {
        /// <summary>
        /// Код результат запроса.
        /// </summary>
        [JsonPropertyName("code")]
        public uint Code { get; set; }

        /// <summary>
        /// Описание результата запроса.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Результаты проверки кодов маркировки.
        /// </summary>
        [JsonPropertyName("codes")]
        public List<CodeInfo> Codes { get; set; }

        /// <summary>
        /// Уникальный идентификатор запроса (UUID).
        /// </summary>
        [JsonPropertyName("reqId")]
        public string? ReqId { get; set; }

        /// <summary>
        /// Время формирования ответа (UTC, миллисекунды).
        /// </summary>
        [JsonPropertyName("reqTimestamp")]
        public ulong ReqTimestamp { get; set; }

        /// <summary>
        /// Признак проверки марки в оффлайн режиме.
        /// </summary>
        [JsonPropertyName("isCheckedOffline")]
        public bool IsCheckedOffline { get; set; }
    }
}