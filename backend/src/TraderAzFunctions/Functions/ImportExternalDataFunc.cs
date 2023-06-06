using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TraderAzFunctions.Entities;

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
        [return: Table("ImportLog")]
        public async Task<ImportLog> Run([TimerTrigger("%ImportExternalDataSchedule%")] TimerInfo myTimer,
                                                IBinder binder,
                                                ILogger log)
        {
            ImportLog result = ImportDataResult(false);

            try
            {
                log.LogDebug($"ImportExternalDataFunc has started at: {DateTime.Now}");

                var response = await httpClient.GetAsync(_uri);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    var data = await responseContent.ReadAsStringAsync();

                    result = ImportDataResult(true);

                    var outboundBlob = new BlobAttribute($"imported-data/{result.RowKey}.json", FileAccess.Write);
                    using (var writer = binder.Bind<TextWriter>(outboundBlob))
                    {
                        writer.Write(data);
                        writer.Flush();
                    }

                    log.LogDebug($"Imported data was stored on the blob in: {outboundBlob.BlobPath}");
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

        private ImportLog ImportDataResult(bool isSucceded)
        {
            return new ImportLog
            {
                PartitionKey = $"{DateTime.Now.Year}:{DateTime.Now.Month.ToString("D2")}",
                RowKey = Guid.NewGuid().ToString(),
           //     Timestamp = DateTimeOffset.Now,
                IsSucceded = isSucceded
            };
        }
    }
}
