namespace Spoleto.Marking.TsPiot.Models
{
    public record SimpleVerificationResult
    {
        /// <summary>
        /// Признак проверки марки в оффлайн режиме.
        /// </summary>
        public bool IsCheckedOffline { get; set; }

        /// <summary>
        /// Признак того, что был получен аварийный код 203
        /// </summary>
        public bool IsEmergencyMode { get; set; }

        /// <summary>
        /// Список КМ
        /// </summary>
        public List<SimpleVerificationResultItem> VerificationResultItems { get; set; }

        /// <summary>
        /// Уникальный идентификатор запроса
        /// </summary>
        public string ReqId { get; set; }

        /// <summary>
        /// Дата и время регистрации запроса (в UTC)
        /// </summary>
        public long? ReqTimestamp { get; set; }

        public int? Code { get; set; }

        public string Message { get; set; }

        public List<string>? Details { get; set; }

        public string RawJson { get; set; }
    }
}
