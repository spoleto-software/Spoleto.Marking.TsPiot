using Spoleto.Marking.TsPiot.Clients;
using Spoleto.Marking.TsPiot.Exceptions;
using Spoleto.Marking.TsPiot.Options;

namespace Spoleto.Marking.TsPiot.Tests
{
    public abstract class TsPiotClientTests
    {
        protected abstract ITsPiotClient GetClient(TsPiotClientOptions settings);

        [Test]
        public async Task CheckSuccessfulCodesTest()
        {
            // Arrange
            var settings = ConfigurationHelper.GetOptions();
            var client = GetClient(settings);
            var codes = ConfigurationHelper.GetSuccessfulCodes();

            // Act
            var res = await client.CheckCodesAsync(codes);

            // Assert
            Assert.That(res, Is.Not.Null);
        }

        [Test]
        public async Task CheckSuccessfulCodesEmergency203Test()
        {
            // Arrange
            var settings = ConfigurationHelper.GetOptions();
            var client = GetClient(settings);
            var codes = ConfigurationHelper.GetSuccessfulCodes203();

            // Act
            var res = await client.CheckCodesAsync(codes);

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res.IsEmergencyMode, Is.True);
        }

        [Test]
        public void CheckUnsuccessfulCodesTest()
        {
            // Arrange
            var settings = ConfigurationHelper.GetOptions();
            var client = GetClient(settings);
            var codes = ConfigurationHelper.GetUnsuccessfulCodes();

            // Act + Assert
            Assert.ThrowsAsync<TsPiotException>(async () =>
            {
                await client.CheckCodesAsync(codes);
            });
        }
    }
}