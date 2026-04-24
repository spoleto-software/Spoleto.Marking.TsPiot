using Grpc.Core;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Spoleto.Marking.TsPiot.Exceptions;
using Spoleto.Marking.TsPiot.Options;

namespace Spoleto.Marking.TsPiot.ResiliencePipelines
{
    public static class GrpcResiliencePipeline
    {
        /// <summary>
        /// Строит <see cref="ResiliencePipeline"/> по настройкам из <see cref="TsPiotClientRetryOptions"/>.<br/><br/>
        ///
        /// Логика:
        /// <list type="bullet">
        /// Повтор вызова на основе gRPC-кодов:
        /// <list type="bullet">
        ///   <item><see cref="StatusCode.Unavailable"/> — сервис временно недоступен.</item>
        ///   <item><see cref="StatusCode.DeadlineExceeded"/> — превышен дедлайн попытки.</item>
        ///   <item><see cref="StatusCode.Internal"/> — внутренняя ошибка сервера.</item>
        /// </list>
        ///   <item>Таймаут одной попытки (<see cref="TsPiotClientRetryOptions.AttemptTimeoutSeconds"/>).</item>
        ///   <item>Общий таймаут цепочки (<see cref="TsPiotClientRetryOptions.TotalTimeoutSeconds"/>).</item>
        /// </list>
        /// </summary>
        public static ResiliencePipeline Build(
            TsPiotClientRetryOptions settings,
            ILogger? logger = null)
        {
            return new ResiliencePipelineBuilder()

            // 1. Общий таймаут всей цепочки (включая повторы)
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(settings.TotalTimeoutSeconds),
                OnTimeout = args =>
                {
                    logger?.LogError(
                        "gRPC: превышен общий таймаут {TotalSeconds} с.",
                        settings.TotalTimeoutSeconds);
                    return default;
                },
            })

            // 2. Retry — только для определенных RpcException
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = settings.RetryCount,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromMilliseconds(settings.BaseDelayMs),
                UseJitter = true,

                // Повторяем только транзиентные gRPC-ошибки и таймаут попытки
                ShouldHandle = new PredicateBuilder()
                    .Handle<RpcException>(ShouldRetry)
                    .Handle<TimeoutRejectedException>(),

                OnRetry = args =>
                {
                    logger?.LogWarning(
                        args.Outcome.Exception,
                        "gRPC retry #{Attempt}/{Max} после {Delay} мс.",
                        args.AttemptNumber + 1,
                        settings.RetryCount,
                        args.RetryDelay.TotalMilliseconds);
                    return default;
                },
            })

            // 3. Таймаут одной попытки
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(settings.AttemptTimeoutSeconds),
                OnTimeout = args =>
                {
                    logger?.LogWarning(
                        "gRPC: таймаут одной попытки {AttemptSeconds} с.",
                        settings.AttemptTimeoutSeconds);
                    return default;
                },
            })

            .Build();
        }

        private static bool ShouldRetry(RpcException ex) => ex.StatusCode switch
        {
            StatusCode.Unavailable => true,
            StatusCode.DeadlineExceeded => true,
            StatusCode.Internal => true,
            _ => false
        };
    }
}
