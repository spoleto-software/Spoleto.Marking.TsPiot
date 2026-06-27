
namespace Spoleto.Marking.TsPiot.Models
{
    /// <summary>
    /// Интерфейс для общего результата проверки кодов маркировки.
    /// </summary>
    public interface ISimpleVerificationResult
    {
        /// <summary>
        /// Признак проверки марки в оффлайн режиме.
        /// </summary>
        bool IsCheckedOffline { get; set; }

        /// <summary>
        /// Признак того, что был получен аварийный код 203.
        /// </summary>
        bool IsEmergencyMode { get; set; }

        /// <summary>
        /// Уникальный идентификатор запроса.
        /// </summary>
        string ReqId { get; set; }

        /// <summary>
        /// Дата и время регистрации запроса (в UTC).
        /// </summary>
        long? ReqTimestamp { get; set; }

        /// <summary>
        /// Код ответа.
        /// </summary>
        int? Code { get; set; }

        /// <summary>
        /// Сообщение об ошибке или статусе.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Детали ошибки или проверки.
        /// </summary>
        List<string>? Details { get; set; }

        /// <summary>
        /// Исходный JSON-ответ.
        /// </summary>
        string RawJson { get; set; }

        /// <summary>
        /// Список КМ.
        /// </summary>
        /// <remarks>
        /// Используем IReadOnlyList для безопасного ковариантного приведения
        /// </remarks>
        IReadOnlyList<ISimpleVerificationResultItem> VerificationResultItems { get; }
    }
}