using FakeItEasy;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shouldly;
using System.Configuration;
using System.Net;
using TraderAzFunctions.Entities;
using TraderAzFunctions.InputDtos;

namespace TraderAzFunctions.Tests.Internall
{
    [TestFixture]
    internal class GetLogFuncTest
    {
        private readonly string? _azFuncUri = ConfigurationManager.AppSettings["AzFuncURI"];

        private readonly string? _tradingServiceUri = ConfigurationManager.AppSettings["TradingServiceURI"];

        private readonly HttpClient httpClient = new HttpClient();

        private string lastImportId; 

        [SetUp]
        public async Task Init()
        {
            // this Init method could be moved to the test base class 
            Environment.SetEnvironmentVariable("ImportURI", _tradingServiceUri);

            var importFunc = new ImportExternalDataFunc();
            var mockedBinder = A.Fake<IBinder>();
            var mockedLogger = A.Fake<ILogger>();
            ImportLog importLog;

            importLog = await importFunc.ImportData(mockedBinder, mockedLogger);
            importLog.ShouldNotBeNull();

            lastImportId = importLog.RowKey;
        }

        [Test]
        public async Task VerifyGetLog()
        {
            StockRecommendationInputDto[]? stockRecommendations = null;

            var logId = new Guid(lastImportId);
            var response = await httpClient.GetAsync(_azFuncUri + $"GetLog?id={logId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var data = await responseContent.ReadAsStringAsync();

                Assert.DoesNotThrow(() => stockRecommendations = JsonConvert.DeserializeObject<StockRecommendationInputDto[]>(data));
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            }
            else
            {
                Assert.Fail("Service is unavaible");
            }
        }

        [Test]
        public async Task InvalidInputParameterShoudReturnBadRequest()
        {
            Guid logId = default;
            var response = await httpClient.GetAsync(_azFuncUri + $"GetLog?id={logId}");

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
        }

        public async Task InvalidInputParameterShoudReturnNotFoundRequest()
        {
            Guid logId = new Guid("c35f3318-5cd8-44b3-b7ff-0ac05f4517fa");

            var response = await httpClient.GetAsync(_azFuncUri + $"GetLog?id={logId}");

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }
    }
}
