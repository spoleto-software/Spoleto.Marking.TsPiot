using System.Text;
using Microsoft.Extensions.Configuration;
using Spoleto.Marking.TsPiot.Options;

namespace Spoleto.Marking.TsPiot.Tests
{
    internal static class ConfigurationHelper
    {
        private static readonly IConfigurationRoot _config;

        static ConfigurationHelper()
        {
            _config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true)
               .AddUserSecrets("c5caa278-50d7-4209-a81c-90f684378601")
               .Build();
        }

        public static TsPiotClientOptions GetOptions()
        {
            var settings = _config.GetSection(nameof(TsPiotClientOptions)).Get<TsPiotClientOptions>();

            return settings;
        }

        public static List<string> GetSuccessfulCodes()
        {
            var codes = _config.GetSection("SuccessfulCodes").Get<List<string>>();

            return codes;
        }

        public static List<string> GetSuccessfulCodes203()
        {
            var codes = _config.GetSection("SuccessfulCodes203").Get<List<string>>();

            return codes;
        }

        public static List<string> GetUnsuccessfulCodes()
        {
            var codes = _config.GetSection("UnsuccessfulCodes").Get<List<string>>();

            return codes;
        }
    }
}
