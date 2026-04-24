namespace Spoleto.Marking.TsPiot.Models
{
    public record SimpleVerificationResult
    {
        /// <summary>
        /// Признак проверки марки в оффлайн режиме.
        /// </summary>
        public bool IsCheckedOffline { get; set; }

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
    }
}
