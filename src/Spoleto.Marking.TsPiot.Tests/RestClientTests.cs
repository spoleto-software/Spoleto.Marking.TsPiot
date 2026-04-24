using Spoleto.Marking.TsPiot.Clients;
using Spoleto.Marking.TsPiot.Options;

namespace Spoleto.Marking.TsPiot.Tests
{
    public class RestClientTests : TsPiotClientTests
    {
        [SetUp]
        public void Setup()
        {
        }

        protected override ITsPiotClient GetClient(TsPiotClientOptions settings) => new TsPiotRestClient(settings);
    }
}