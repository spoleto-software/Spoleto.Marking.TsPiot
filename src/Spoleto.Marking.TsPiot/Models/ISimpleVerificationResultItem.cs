namespace Spoleto.Marking.TsPiot.Models
{
    /// <summary>
    /// Интерфейс для элемента результата проверки кода маркировки.
    /// </summary>
    public interface ISimpleVerificationResultItem
    {
        /// <summary>
        /// КМ из запроса.
        /// </summary>
        string MarkingCode { get; set; }

        /// <summary>
        /// Можно продавать или нет.
        /// </summary>
        bool Success { get; set; }

        /// <summary>
        /// Идентификатор экземпляра ПО "Локальный модуль "Честный знак"".
        /// </summary>
        string Inst { get; set; }

        /// <summary>
        /// Версия базы «чёрного списка», на которой выполнялась проверка КИ.
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Детальный ответ от ЦРПТ. Полный json исходного полного объекта.
        /// </summary>
        string AdditionalInfo { get; set; }
    }
}