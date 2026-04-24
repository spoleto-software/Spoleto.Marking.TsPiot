using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.Models
{
    public record ClientInfo
    {
        /// <summary>
        /// Наименование ПМСР (кассового ПО).
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Версия ПМСР (кассового ПО).
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        /// Идентификатор ПМСР в реестре ГИС МТ.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Контрольная сумма / ЭЦП исполняемого файла ПМСР.
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
