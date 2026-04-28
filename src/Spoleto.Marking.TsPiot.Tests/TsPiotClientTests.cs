using Spoleto.Marking.TsPiot.Clients;
using Spoleto.Marking.TsPiot.Exceptions;
using Spoleto.Marking.TsPiot.Extensions;
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
            var simple = res.AsSimpleResult();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(res, Is.Not.Null);
                Assert.That(simple, Is.Not.Null);
            });
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
            var simple = res.AsSimpleResult();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(res.IsEmergencyMode, Is.True);
                Assert.That(simple, Is.Not.Null);
            });
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

        [Test]
        public async Task GetInfoTest()
        {
            // Arrange
            var settings = ConfigurationHelper.GetOptions();
            var client = GetClient(settings);

            // Act
            var res = await client.GetInfoAsync();

            // Assert
            Assert.That(res, Is.Not.Null);
        }
    }
}