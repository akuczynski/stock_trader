using Newtonsoft.Json;
using Shouldly;
using System.Configuration;
using System.Net;
using TraderAzFunctions.InputDtos;
using TraderAzFunctions.OutputDtos;

namespace TraderAzFunctions.Tests.Internall
{
    [TestFixture]
    internal class GetLogFuncTest
    {
        private readonly string? _azFuncUri = ConfigurationManager.AppSettings["AzFuncURI"];

        private readonly HttpClient httpClient = new HttpClient();

        private string lastImportId; 

        [SetUp]
        public async Task Init()
        {
            // this allows to trigger Non-HTTP triggered functions 
            var response = await httpClient.GetAsync(_azFuncUri + $"admin/functions/ImportExternalData");

            DateTime dateFrom = DateTime.Now.AddMinutes(-1);
            DateTime dateTo = DateTime.Now;
            ImportLogOutputDto[]? logEntries = null;

            response = await httpClient.GetAsync(_azFuncUri + $"api/GetAllLogs?from={dateFrom}&to={dateTo}");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var data = await responseContent.ReadAsStringAsync();

                Assert.DoesNotThrow(() => logEntries = JsonConvert.DeserializeObject<ImportLogOutputDto[]>(data));
                lastImportId = logEntries.FirstOrDefault(x => x.IsSucceded).Id;
            }
        }

        [Test]
        public async Task VerifyGetLog()
        {
            StockRecommendationInputDto[]? stockRecommendations = null;

            var logId = new Guid(lastImportId);
            var response = await httpClient.GetAsync(_azFuncUri + $"api/GetLog?id={logId}");

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
            var response = await httpClient.GetAsync(_azFuncUri + $"api/GetLog?id={logId}");

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task InvalidInputParameterShoudReturnNotFoundRequest()
        {
            Guid logId = new Guid("c35f3318-5cd8-44b3-b7ff-0ac05f4517fa");

            var response = await httpClient.GetAsync(_azFuncUri + $"api/GetLog?id={logId}");

            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }
    }
}
