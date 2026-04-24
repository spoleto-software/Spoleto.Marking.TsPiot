using Spoleto.Marking.TsPiot.Models;

namespace Spoleto.Marking.TsPiot.Clients
{
    public interface ITsPiotClient
    {
        Task<CodesCheckResult> CheckCodesAsync(IEnumerable<string> codes, CancellationToken cancellationToken = default);

        Task<TsPiotInfo> GetInfoAsync(CancellationToken cancellationToken = default);
    }
}
