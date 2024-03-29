﻿using Newtonsoft.Json;
using Shouldly;
using System.Configuration;
using TraderAzFunctions.InputDtos;

namespace TraderAzFunctions.Tests.Externall
{
    [TestFixture]
    public class TradingServiceTests
    {
        private readonly string? _uri = ConfigurationManager.AppSettings["RealTradingServiceURI"];

        private readonly HttpClient httpClient = new HttpClient();

        [Test]
        public async Task VerifyCallToExternalAPI()
        {
            var response = await httpClient.GetAsync(_uri);
            StockRecommendationInputDto[]? stockRecommendations = null;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var data = await responseContent.ReadAsStringAsync();

                Assert.DoesNotThrow(() => stockRecommendations = JsonConvert.DeserializeObject<StockRecommendationInputDto[]>(data));
                stockRecommendations.ShouldNotBeEmpty();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                // if REST api returned "429" status code then don't make any assertions  
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Service is unavaible");
            }
        }
    }
}
