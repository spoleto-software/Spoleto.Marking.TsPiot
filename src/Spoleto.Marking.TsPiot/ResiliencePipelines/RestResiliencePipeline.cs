using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Spoleto.Marking.TsPiot.Exceptions;
using Spoleto.Marking.TsPiot.Options;
using Spoleto.RestClient;

namespace Spoleto.Marking.TsPiot.ResiliencePipelines
{
    public static class RestResiliencePipeline
    {
        /// <summary>
        /// Строит <see cref="ResiliencePipeline{TextRestResponse}"/> по настройкам из <see cref="TspiotClientRetryOptions"/>.<br/><br/>
        ///
        /// Логика:
        /// <list type="bullet">
        ///   <item>200-е коды → успех, без повтора.</item>
        ///   <item>400 → <see cref="TsPiotNoRetryException"/>, без повтора.</item>
        ///   <item>500, 514 → повтор с экспоненциальным backoff.</item>
        ///   <item>Таймаут одной попытки (<see cref="TspiotClientRetryOptions.AttemptTimeoutSeconds"/>).</item>
        ///   <item>Общий таймаут цепочки (<see cref="TspiotClientRetryOptions.TotalTimeoutSeconds"/>).</item>
        /// </list>
        /// </summary>
        public static ResiliencePipeline<TextRestResponse> Build(
            TspiotClientRetryOptions settings,
            ILogger? logger = null)
        {
            return new ResiliencePipelineBuilder<TextRestResponse>()

                // ── 1. Общий таймаут всей цепочки ─────────────────────────────────
                .AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(settings.TotalTimeoutSeconds),
                    OnTimeout = args =>
                    {
                        logger?.LogError("ТС ПИоТ REST: превышен общий таймаут {TotalSeconds} с.", settings.TotalTimeoutSeconds);

                        return default;
                    },
                })

                // ── 2. Retry с кастомной логикой по кодам ответа ──────────────────
                .AddRetry(new RetryStrategyOptions<TextRestResponse>
                {
                    MaxRetryAttempts = settings.RetryCount,
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromMilliseconds(settings.BaseDelayMs),
                    UseJitter = true,

                    // Условие повтора: исключение транспорта ИЛИ «повторяемый» статус
                    ShouldHandle = new PredicateBuilder<TextRestResponse>()
                        .Handle<HttpRequestException>()
                        .Handle<TimeoutRejectedException>()          // таймаут одной попытки
                        .HandleResult(response =>
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                return false;
                            }

                            int code = (int)response.StatusCode;
                            if (ShouldRetry(code))
                            {
                                return true;
                            }

                            // Остальные коды не повторять никогда, например, 400
                            throw new TsPiotNoRetryException($"HTTP Response code {code}: повтор запрещён политикой.", new Exception(response.ToString()));
                        }),

                    OnRetry = args =>
                    {
                        int? code = args.Outcome.Result is not null
                            ? (int)args.Outcome.Result.StatusCode
                            : null;

                        logger?.LogWarning(
                            "ТС ПИоТ REST: повтор #{Attempt}/{Max}. " +
                            "HTTP-код: {Code}. Задержка: {Delay} мс. Причина: {Reason}",
                            args.AttemptNumber + 1,
                            settings.RetryCount,
                            code?.ToString(),
                            args.RetryDelay.TotalMilliseconds,
                            args.Outcome.Exception?.Message);

                        return default;
                    },
                })

                // ── 3. Таймаут одной попытки ──────────────────────────────────────
                .AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(settings.AttemptTimeoutSeconds),
                    OnTimeout = args =>
                    {
                        logger?.LogWarning("ТС ПИоТ REST: таймаут попытки {AttemptSeconds} с.", settings.AttemptTimeoutSeconds);

                        return default;
                    },
                })

                .Build();
        }

        private static bool ShouldRetry(int responseStatusCode) => responseStatusCode switch
        {
            500 => true,
            504 => true,
            514 => true,
            _ => false
        };
    }
}
