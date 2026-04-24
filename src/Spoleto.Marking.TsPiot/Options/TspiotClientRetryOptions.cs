namespace Spoleto.Marking.TsPiot.Options
{
    public record TsPiotClientRetryOptions
    {
        /// <summary>
        /// Таймаут одной HTTP-попытки в секундах.
        /// </summary>
        public int AttemptTimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// Общий таймаут всей цепочки вызовов (включая все повторы) в секундах.
        /// </summary>
        public int TotalTimeoutSeconds { get; set; } = 180;

        /// <summary>
        /// Базовая задержка между попытками в миллисекундах.
        /// Реальная задержка рассчитывается как: BaseDelayMs * 2^attempt (экспоненциальный backoff).
        /// </summary>
        public int BaseDelayMs { get; set; } = 500;

        /// <summary>
        /// Максимальное количество повторных попыток (не считая первый вызов).
        /// </summary>
        public int RetryCount { get; set; } = 3;
    }
}
