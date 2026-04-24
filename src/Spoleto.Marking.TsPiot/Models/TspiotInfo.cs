using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.Models
{
    /// <summary>
    /// Ответ на получение информации о ККТ, на которую зарегистрирован ТС ПИоТ
    /// </summary>
    public record TsPiotInfo
    {
        /// <summary>
        /// Идентификатор ТС ПИоТ.
        /// </summary>
        [JsonPropertyName("TsPiotId")]
        public string TsPiotId { get; set; } 

        /// <summary>
        /// Серийный номер ККТ.
        /// </summary>
        [JsonPropertyName("kktSerial")]
        public string KktSerial { get; set; }

        /// <summary>
        /// Серийный номер ФН.
        /// </summary>
        [JsonPropertyName("fnSerial")]
        public string FnSerial { get; set; }

        /// <summary>
        /// ИНН ККТ.
        /// </summary>
        [JsonPropertyName("kktInn")]
        public string KktInn { get; set; } = default!;

        /// <summary>
        /// Таймаут проверки КМ (мс).
        /// </summary>
        [JsonPropertyName("codesCheckTimeout")]
        public uint CodesCheckTimeout { get; set; }
    }
}
