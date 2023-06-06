using FakeItEasy;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shouldly;
using System.Configuration;
using System.Net;
using TraderAzFunctions.Entities;
using TraderAzFunctions.OutputDtos;

namespace TraderAzFunctions.Tests.Internall
{
    [TestFixture]
    internal class GetAllLogsFuncTests
    {
        private readonly string? _azFuncUri = ConfigurationManager.AppSettings["AzFuncURI"];

        private readonly string? _tradingServiceUri = ConfigurationManager.AppSettings["TradingServiceURI"];

        private readonly HttpClient httpClient = new HttpClient();

        [SetUp]
        public async Task Init()
        {
            Environment.SetEnvironmentVariable("ImportURI", _tradingServiceUri);

            var importFunc = new ImportExternalDataFunc();
            var mockedBinder = A.Fake<IBinder>();
            var mockedLogger = A.Fake<ILogger>();
            ImportLog importLog;

            importLog = await importFunc.ImportData(mockedBinder, mockedLogger);
            importLog.ShouldNotBeNull(); 
        }

        [Test]
        public async Task VerifyGetAllLogs()
        {
            ImportLogOutputDto[]? logEntries = null;

            DateTime dateFrom = DateTime.Now.AddHours(-1);
            DateTime dateTo = DateTime.Now;

            var response = await httpClient.GetAsync(_azFuncUri + $"GetAllLogs?from={dateFrom}&to={dateTo}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var data = await responseContent.ReadAsStringAsync();

                Assert.DoesNotThrow(() => logEntries = JsonConvert.DeserializeObject<ImportLogOutputDto[]>(data));
                response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            }
            else
            {
                Assert.Fail("Service is unavaible");
            }
        }

        [Test]
        public async Task InvalidInputParametsShoudReturnBadRequest()
        {
            DateTime dateFrom = DateTime.Now.AddHours(1);
            DateTime dateTo = DateTime.Now;

            var response = await httpClient.GetAsync(_azFuncUri + $"GetAllLogs?from={dateFrom}&to={dateTo}");

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest); 
        }
    }
}
