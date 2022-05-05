using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using nhsapp.sample.web.integration.ViewModels;
using NUnit.Framework;
using Moq;

namespace nhsapp.sample.web.integration.UnitTests.ViewModels
{
    [TestFixture]
    public class AdobeAnalyticsViewModelTests
    {
        private AdobeAnalyticsViewModel _viewModel;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<HttpContext> _mockContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.SetupGet(s => s["AdobeAnalyticsScriptUrl"]).Returns("www.test.com");


            _mockContext = new Mock<HttpContext>();
            _mockContext.Setup(x => x.Request.PathBase).Returns(new PathString("/service-name"));

            _viewModel = new AdobeAnalyticsViewModel(_mockContext.Object, _mockConfiguration.Object);
        }

        [Test]
        public void AdobeAnalytics_ViewModel_Test()
        {
            Assert.AreEqual(_viewModel.ScriptUrl, "www.test.com");
            Assert.AreEqual(_viewModel.PageName, "nhs:web:service-name");
            Assert.AreEqual(_viewModel.Categories, @"{""primaryCategory"":""service-name""}");
        }
    }
}
