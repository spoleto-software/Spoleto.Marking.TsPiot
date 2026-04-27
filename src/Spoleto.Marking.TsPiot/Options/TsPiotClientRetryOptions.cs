namespace Spoleto.Marking.TsPiot.Options
{
    public record TsPiotClientRetryOptions
    {
        /// <summary>
        /// Таймаут одной HTTP-попытки в секундах.
        /// </summary>
        public int AttemptTimeoutSeconds { get; init; } = 30;

        /// <summary>
        /// Общий таймаут всей цепочки вызовов (включая все повторы) в секундах.
        /// </summary>
        public int TotalTimeoutSeconds { get; init; } = 180;

        /// <summary>
        /// Базовая задержка между попытками в миллисекундах.
        /// Реальная задержка рассчитывается как: BaseDelayMs * 2^attempt (экспоненциальный backoff).
        /// </summary>
        public int BaseDelayMs { get; init; } = 500;

        /// <summary>
        /// Максимальное количество повторных попыток (не считая первый вызов).
        /// </summary>
        public int RetryCount { get; init; } = 3;
    }
}
