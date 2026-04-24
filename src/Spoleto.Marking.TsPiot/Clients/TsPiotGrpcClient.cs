using System.Net.Security;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;
using Spoleto.Marking.TsPiot.Exceptions;
using Spoleto.Marking.TsPiot.Extensions;
using Spoleto.Marking.TsPiot.Models;
using Spoleto.Marking.TsPiot.Options;
using Spoleto.Marking.TsPiot.ResiliencePipelines;


namespace Spoleto.Marking.TsPiot.Clients
{
    public sealed class TsPiotGrpcClient : ITsPiotClient, IDisposable
    {
        private readonly GrpcChannel _channel;
        private readonly Spoleto.Marking.TsPiot.Grpc.CodesCheckService.CodesCheckServiceClient _client;
        private readonly TsPiotClientOptions _settings;
        private readonly ILogger? _logger;
        private readonly ResiliencePipeline _pipeline;

        public TsPiotGrpcClient(TsPiotClientOptions settings, ILogger? logger = null)
        {
            _settings = settings;
            _logger = logger;

            var httpHandler = new SocketsHttpHandler();

#if DEBUG
            // Return true to allow any certificate
            httpHandler.SslOptions = new SslClientAuthenticationOptions
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                {
                    return true;
                }
            };

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
#endif

            // allow connections to the same server:
            httpHandler.EnableMultipleHttp2Connections = true;

            _channel = GrpcChannel.ForAddress(settings.ServiceUrl, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            _client = new Grpc.CodesCheckService.CodesCheckServiceClient(_channel);

            _pipeline = GrpcResiliencePipeline.Build(settings.RetryOptions, logger);
        }


        public async Task<CodesCheckResult> CheckCodesAsync(IEnumerable<string> codes, CancellationToken cancellationToken = default)
        {
            var request = new Grpc.CodesCheckRequest
            {
                ClientInfo = _settings.AppOptions.ToGrpcClientInfo()
            };

            request.Codes.AddRange(codes);

            var res = await ExecuteAsync(async (deadline, ct) =>
            {
                _logger?.LogInformation("Проверка {Count} КМ.", request.Codes.Count);

                var response = await _client.CheckCodesAsync(
                    request,
                    deadline: deadline,
                    cancellationToken: ct).ConfigureAwait(false);

                LogResult(response);

                return response;
            },
            cancellationToken).ConfigureAwait(false);

            return res.ToDto();
        }

        public async Task<TsPiotInfo> GetInfoAsync(CancellationToken cancellationToken = default)
        {
            var res = await ExecuteAsync(async (deadline, ct) =>
            {
                _logger?.LogInformation("Запрос информации о ТС ПИоТ.");

                return await _client.GetTsPiotInfoAsync(
                    new Google.Protobuf.WellKnownTypes.Empty(),
                    deadline: deadline,
                    cancellationToken: ct).ConfigureAwait(false);
            },
            cancellationToken).ConfigureAwait(false);

            return res.ToDto();
        }

        private async Task<T> ExecuteAsync<T>(Func<DateTime?, CancellationToken, Task<T>> action, CancellationToken cancellationToken)
        {
            var deadline = DateTime.UtcNow.AddSeconds(_settings.RetryOptions.TotalTimeoutSeconds);

            try
            {
                return await _pipeline.ExecuteAsync(
                    async resilienceCt =>
                    {
                        try
                        {
                            return await action(deadline, resilienceCt).ConfigureAwait(false);
                        }
                        catch (RpcException ex)
                        {
                            _logger?.LogError(ex, "gRPC ошибка: {Status} — {Detail}",
                                ex.StatusCode,
                                ex.Status.Detail);

                            throw;
                        }
                    },
                    cancellationToken).ConfigureAwait(false);
            }
            catch (TimeoutRejectedException ex)
            {
                throw new TsPiotException($"Превышен общий таймаут gRPC-вызова ({_settings.RetryOptions.TotalTimeoutSeconds} с).", ex);
            }
        }

        private void LogResult(Grpc.CodesCheckResult result)
        {
            switch (result.ResultCase)
            {
                case Grpc.CodesCheckResult.ResultOneofCase.CodesResponse:
                    _logger?.LogInformation("КМ успешно проверены.");
                    break;

                case Grpc.CodesCheckResult.ResultOneofCase.Error:
                    _logger?.LogWarning(
                        "Ошибка: {Code} — {Message}",
                        result.Error.ErrorCode,
                        result.Error.ErrorDescription);
                    break;

                default:
                    _logger?.LogWarning("Неизвестный тип ответа от ТС ПИоТ.");
                    break;
            }
        }



        public void Dispose() => _channel.Dispose();
    }
}
