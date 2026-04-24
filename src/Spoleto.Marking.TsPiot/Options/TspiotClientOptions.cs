namespace Spoleto.Marking.TsPiot.Options
{
    public record TsPiotClientOptions
    {
        public string ServiceUrl { get; set; }

        public TspiotClientRetryOptions RetryOptions { get; set; }

        public TsPiotClientAppOptions AppOptions { get; set; }
    }
}
