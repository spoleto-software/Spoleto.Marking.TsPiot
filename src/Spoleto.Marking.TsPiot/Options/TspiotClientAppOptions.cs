namespace Spoleto.Marking.TsPiot.Options
{
    public record TsPiotClientAppOptions
    {
        /// <summary>
        /// Наименование ПМСР (кассового ПО).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Версия ПМСР (кассового ПО).
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Идентификатор ПМСР в реестре ГИС МТ.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Контрольная сумма / ЭЦП исполняемого файла ПМСР.
        /// </summary>
        public string Token { get; set; }
    }
}
