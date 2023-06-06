using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TraderAzFunctions.InputDtos; 

namespace TraderAzFunctions
{
    public class ImportExternalDataFunc
    {
        // HttpClient should be defined as static field to avoid connection exceeded issue.
        // Please check following MS DOC for more information:
        // https://learn.microsoft.com/en-us/azure/azure-functions/manage-connections?tabs=csharp
        private static HttpClient httpClient = new HttpClient();

        private static readonly string _uri = Environment.GetEnvironmentVariable("ImportURI"); 

        [FunctionName("ImportExternalDataFunc")]
        [return: Table("ImportAttempts")]
        public async Task<ImportAttempt> Run([TimerTrigger("%ImportExternalDataSchedule%")] TimerInfo myTimer, ILogger log)
        {
            ImportAttempt result = ImportDataResult(false);

            try
            {
                log.LogDebug($"ImportExternalDataFunc has started at: {DateTime.Now}");

                var response = await httpClient.GetAsync(_uri);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    var data = await responseContent.ReadAsStringAsync();

                    var tradeRecommendations = JsonConvert.DeserializeObject<TradeRecommendations[]>(data);

                    result = ImportDataResult(true);
                }

            }
            catch (Exception ex)
            {
                log.LogError($"ImportExternalDataFunc got exception: {ex.Message}");
                result = ImportDataResult(false);
            }

            log.LogDebug($"ImportExternalDataFunc has finished at: {DateTime.Now}");
            return result;
        }

        private ImportAttempt ImportDataResult(bool isSucceded)
        {
            return new ImportAttempt
            {
                PartitionKey = $"{DateTime.Now.Year}:{DateTime.Now.Month.ToString("D2")}",
                RowKey = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now,
                IsSucceded = isSucceded
            };
        }


        public class ImportAttempt
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public DateTimeOffset? Timestamp { get; set; }
            public bool IsSucceded { get; set; }
        }
    }
}
