namespace Spoleto.Marking.TsPiot.Options
{
    public record TsPiotClientOptions
    {
        public string ServiceUrl { get; init; }

        public TsPiotClientRetryOptions RetryOptions { get; init; } = new();

        public TsPiotClientAppOptions AppOptions { get; init; }
    }
}
