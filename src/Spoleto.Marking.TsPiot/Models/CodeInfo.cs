using System.Text.Json.Serialization;

namespace Spoleto.Marking.TsPiot.Models
{
    public record CodeInfo
    {
        /// <summary>
        /// КИ / КиЗ из запроса.
        /// </summary>
        [JsonPropertyName("cis")]
        public string Cis { get; set; }

        /// <summary>
        /// Признак наличия кода в ГИС МТ.
        /// true — найден, false — не найден.
        /// </summary>
        [JsonPropertyName("found")]
        public bool Found { get; set; }

        /// <summary>
        /// Валидность структуры КИ / КиЗ.
        /// </summary>
        [JsonPropertyName("valid")]
        public bool Valid { get; set; }

        /// <summary>
        /// КИ без крипто-подписи.
        /// </summary>
        [JsonPropertyName("printView")]
        public string PrintView { get; set; }

        /// <summary>
        /// GTIN товара.
        /// </summary>
        [JsonPropertyName("gtin")]
        public string Gtin { get; set; }

        /// <summary>
        /// Идентификаторы товарных групп.
        /// </summary>
        [JsonPropertyName("groupIds")]
        public List<uint> GroupIds { get; set; } = new();

        /// <summary>
        /// Результат проверки крипто-подписи.
        /// </summary>
        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        /// <summary>
        /// Можно ли продавать (в статусе "В обороте").
        /// </summary>
        [JsonPropertyName("realizable")]
        public bool Realizable { get; set; }

        /// <summary>
        /// Признак нанесения КМ на упаковку.
        /// </summary>
        [JsonPropertyName("utilised")]
        public bool Utilised { get; set; }

        /// <summary>
        /// Срок годности.
        /// Формат: yyyy-MM-ddTHH:mm:ss.SSSZ
        /// </summary>
        [JsonPropertyName("expireDate")]
        public string ExpireDate { get; set; }

        /// <summary>
        /// Переменный срок годности (для молочки).
        /// Формат: индекс -> дата.
        /// </summary>
        [JsonPropertyName("variableExpirations")]
        public string VariableExpirations { get; set; }

        /// <summary>
        /// Дата производства.
        /// </summary>
        [JsonPropertyName("productionDate")]
        public string ProductionDate { get; set; }

        /// <summary>
        /// Вес (в граммах).
        /// Только для молочной продукции.
        /// </summary>
        [JsonPropertyName("productWeight")]
        public double? ProductWeight { get; set; }

        /// <summary>
        /// Ветеринарный документ (для молочки).
        /// </summary>
        [JsonPropertyName("prVetDocument")]
        public string? PrVetDocument { get; set; }

        /// <summary>
        /// Код принадлежит указанному ИНН.
        /// </summary>
        [JsonPropertyName("isOwner")]
        public bool? IsOwner { get; set; }

        /// <summary>
        /// Продажа заблокирована.
        /// </summary>
        [JsonPropertyName("isBlocked")]
        public bool IsBlocked { get; set; }

        ///// <summary>
        ///// Проверка выполнена оффлайн.
        ///// </summary>
        //[JsonPropertyName("isCheckedOffline")]
        //public bool? IsCheckedOffline { get; set; }

        /// <summary>
        /// Органы, установившие блокировку.
        /// (RAR, FTS, FNS и т.д.)
        /// </summary>
        [JsonPropertyName("ogvs")]
        public List<string>? Ogvs { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        [JsonPropertyName("message")]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Код ошибки (0 — нет ошибки).
        /// </summary>
        [JsonPropertyName("errorCode")]
        public uint? ErrorCode { get; set; }

        /// <summary>
        /// Контроль прослеживаемости включен.
        /// </summary>
        [JsonPropertyName("isTracking")]
        public bool IsTracking { get; set; }

        /// <summary>
        /// Товар продан.
        /// </summary>
        [JsonPropertyName("sold")]
        public bool Sold { get; set; }

        /// <summary>
        /// Признак использования причин выбытия, разрешающих продажу КМ
        /// </summary>
        [JsonPropertyName("eliminationState")]
        public uint? EliminationState { get; set; }

        /// <summary>
        /// Максимальная розничная цена (в копейках).
        /// </summary>
        [JsonPropertyName("mrp")]
        public uint? Mrp { get; set; }

        /// <summary>
        /// Минимальная цена (в копейках).
        /// </summary>
        [JsonPropertyName("smp")]
        public long? Smp { get; set; }

        /// <summary>
        /// Признак "серой зоны".
        /// </summary>
        [JsonPropertyName("grayZone")]
        public bool? GrayZone { get; set; }

        /// <summary>
        /// Количество единиц / объем / вес.
        /// </summary>
        [JsonPropertyName("innerUnitCount")]
        public uint? InnerUnitCount { get; set; }

        /// <summary>
        /// Счётчик проданного и возвращённого товара
        /// </summary>
        [JsonPropertyName("soldUnitCount")]
        public uint? SoldUnitCount { get; set; }

        /// <summary>
        /// Тип упаковки.
        /// </summary>
        [JsonPropertyName("packageType")]
        public string PackageType { get; set; }

        /// <summary>
        /// Родительский КИ (агрегат).
        /// </summary>
        [JsonPropertyName("parent")]
        public string? Parent { get; set; }

        /// <summary>
        /// ИНН производителя.
        /// </summary>
        [JsonPropertyName("producerInn")]
        public string? ProducerInn { get; set; }

        /// <summary>
        /// Серийный номер партии.
        /// </summary>
        [JsonPropertyName("productionSerialNumber")]
        public string? ProductionSerialNumber { get; set; }

        /// <summary>
        /// Номер партии.
        /// </summary>
        [JsonPropertyName("productionBatchNumber")]
        public string? ProductionBatchNumber { get; set; }

        /// <summary>
        /// Заводской серийный номер.
        /// </summary>
        [JsonPropertyName("factorySerialNumber")]
        public string? FactorySerialNumber { get; set; }

        /// <summary>
        /// Вместимость упаковки.
        /// </summary>
        [JsonPropertyName("packageQuantity")]
        public uint? PackageQuantity { get; set; }

        /// <summary>
        /// ID локального модуля Честный ЗНАК.
        /// </summary>
        [JsonPropertyName("inst")]
        public string? Inst { get; set; }

        /// <summary>
        /// Версия локального модуля.
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; set; }
    }
}
