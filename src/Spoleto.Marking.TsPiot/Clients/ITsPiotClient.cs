using Spoleto.Marking.TsPiot.Models;

namespace Spoleto.Marking.TsPiot.Clients
{
    public interface ITsPiotClient
    {
        CodesCheckResult CheckCodes(IEnumerable<string> codes, CancellationToken cancellationToken = default)
            => CheckCodesAsync(codes, cancellationToken).GetAwaiter().GetResult();

        Task<CodesCheckResult> CheckCodesAsync(IEnumerable<string> codes, CancellationToken cancellationToken = default);

        TsPiotInfo GetInfo(CancellationToken cancellationToken = default)
            => GetInfoAsync(cancellationToken).GetAwaiter().GetResult();

        Task<TsPiotInfo> GetInfoAsync(CancellationToken cancellationToken = default);
    }
}
