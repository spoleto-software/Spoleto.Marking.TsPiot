namespace Spoleto.Marking.TsPiot.Models
{
    public record SimpleVerificationResultItem
    {
        /// <summary>
        /// КМ из запроса
        /// </summary>
        public string MarkingCode { get; set; }

        /// <summary>
        /// Можно продавать или нет
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Идентификатор экземпляра ПО "Локальный модуль "Честный знак""
        /// </summary>
        public string Inst { get; set; }

        /// <summary>
        /// Версия базы «чёрного списка», на которой выполнялась проверка КИ
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Детальный ответ от црпт. Полный json исходного полного объекта.
        /// </summary>
        public string AdditionalInfo { get; set; }
    }
}
