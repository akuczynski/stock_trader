using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using System.Configuration;
using TraderAzFunctions.InputDtos; 

namespace TraderAzFunctions.Tests
{
    [TestFixture]
    public class ExternallAPITradestieTests 
    {
        private readonly string? _uri = ConfigurationManager.AppSettings["tradeURI"]; 

        private readonly HttpClient httpClient = new HttpClient();
        
        [Test]
        public async Task CheckGetRedditCall()
        {
            var response = await httpClient.GetAsync(_uri);
            TradeRecommendations[]? tradeRecommendations = null;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var data = await responseContent.ReadAsStringAsync();

                Assert.DoesNotThrow(() => tradeRecommendations = JsonConvert.DeserializeObject<TradeRecommendations[]>(data));
                tradeRecommendations.ShouldNotBeEmpty();
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
