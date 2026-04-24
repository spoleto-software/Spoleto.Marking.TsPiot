using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;
using Spoleto.Marking.TsPiot.Exceptions;
using Spoleto.Marking.TsPiot.Extensions;
using Spoleto.Marking.TsPiot.Models;
using Spoleto.Marking.TsPiot.Options;
using Spoleto.Marking.TsPiot.ResiliencePipelines;
using Spoleto.RestClient;
using Spoleto.RestClient.Serializers;

namespace Spoleto.Marking.TsPiot.Clients
{
    public sealed class TsPiotRestClient : ITsPiotClient, IDisposable
    {
        private readonly IRestClient _restClient;
        private readonly TsPiotClientOptions _settings;
        private readonly ILogger? _logger;
        private readonly ResiliencePipeline<TextRestResponse> _pipeline;

        /// <summary>
        /// Простой конструктор — создаёт собственный <see cref="HttpClient"/>.
        /// </summary>  
        /// <param name="settings">Настройки клиента и политики запросов.</param>
        /// <param name="logger">Опциональный логгер.</param>
        public TsPiotRestClient(TsPiotClientOptions settings, ILogger? logger = null)
        {
            _settings = settings;
            _logger = logger;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(settings.ServiceUrl),
                Timeout = TimeSpan.FromSeconds(_settings.RetryOptions.TotalTimeoutSeconds)
            };

            _restClient = new RestClientFactory().WithHttpClient(httpClient, true).Build();

            _pipeline = RestResiliencePipeline.Build(_settings.RetryOptions, logger);
        }

        /// <summary>
        /// Конструктор для DI / <c>IHttpClientFactory</c>.
        /// </summary>
        /// <param name="httpClient">Внешний <see cref="HttpClient"/> (управление временем жизни на стороне DI).</param>
        /// <param name="settings">Настройки клиента и политики запросов.</param>
        /// <param name="logger">Логгер.</param>
        public TsPiotRestClient(HttpClient httpClient, TsPiotClientOptions settings, ILogger<TsPiotRestClient>? logger = null)
        {
            _settings = settings;
            _logger = logger;

            _restClient = new RestClientFactory().WithHttpClient(httpClient, false).Build();

            _pipeline = RestResiliencePipeline.Build(_settings.RetryOptions, logger);
        }

        public async Task<CodesCheckResult> CheckCodesAsync(IEnumerable<string> codes, CancellationToken cancellationToken = default)
        {
            var request = new CodesCheckRequest
            {
                ClientInfo = _settings.AppOptions.ToRestClientInfo()
            };

            request.Codes.AddRange(codes);

            _logger?.LogInformation("Проверка {Count} КМ.", request.Codes.Count);

            var res = await ExecuteAsync<CodesCheckResult>(() => new RestRequestFactory(RestHttpMethod.Post, "api/v1/codes/check").ThrowIfHttpError(false).WithJsonContent(request).Build(), cancellationToken).ConfigureAwait(false);

            return res;
        }

        public async Task<TsPiotInfo> GetInfoAsync(CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Запрос информации о ТС ПИоТ.");

            var res = await ExecuteAsync<TsPiotInfo>(() => new RestRequestFactory(RestHttpMethod.Post, "api/v1/info").Build(), cancellationToken).ConfigureAwait(false);

            return res;
        }

        /// <summary>
        /// Выполняет HTTP-запрос через Polly.
        /// Фабрика <paramref name="requestFactory"/> вызывается на каждой попытке,
        /// поскольку <see cref="HttpRequestMessage"/> не допускает повторной отправки.
        /// </summary>
        private async Task<T> ExecuteAsync<T>(Func<RestRequest> requestFactory, CancellationToken ct) where T : class
        {
            TextRestResponse response;

            try
            {
                response = await _pipeline.ExecuteAsync(
                    async resilienceCt =>
                    {
                        var restRequest = requestFactory();

                        var restResponse = await _restClient.ExecuteAsStringAsync(restRequest, resilienceCt).ConfigureAwait(false);

                        return restResponse;
                    },
                    ct).ConfigureAwait(false);
            }
            catch (TsPiotNoRetryException)
            {
                throw; // прокидываем без обёртки — отдельный тип исключения
            }
            catch (TimeoutRejectedException ex)
            {
                throw new TsPiotException("Превышен общий таймаут обращения к сервису ТС ПИоТ.", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new TsPiotException($"Транспортная ошибка при обращении к ТС ПИоТ: {ex.Message}", ex);
            }

            EnsureSuccess(response);

            var json = response?.Content ?? throw new TsPiotException($"Пустой ответ от сервера.");

            var objectResult = SerializationManager.Deserialize<T>(json);

            return objectResult;
        }

        /// <summary>
        /// Проверяет итоговый HTTP-ответ после завершения всех попыток.
        /// 2xx — успех. Остальное — <see cref="TsPiotException"/>.
        /// </summary>
        private static void EnsureSuccess(TextRestResponse response)
        {
            if (response.IsSuccessStatusCode)
                return;

            int code = (int)response.StatusCode;
            var body = response.Content;

            throw new TsPiotException($"HTTP {code} {response.ReasonPhrase}: {body}");
        }

        public void Dispose() => _restClient.Dispose();
    }
}
