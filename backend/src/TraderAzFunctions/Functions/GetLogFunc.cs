using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Web.Http;

namespace TraderAzFunctions
{
    /// <summary>
    /// This Az Func returns payload of the specific log entry.  
    /// </summary>
    public static class GetLogFunc
    {
        [FunctionName("GetLog")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            IBinder binder,
            ILogger log)
        {
            try
            {
                string inputId = req.Query["id"];
                if (string.IsNullOrEmpty(inputId) || !Guid.TryParse(inputId, out var blobId))
                {
                    return new BadRequestErrorMessageResult("\"id\" parameter was not send or has invalid format!");
                }

                var inputBlob = new BlobAttribute($"imported-data/{blobId}.json", FileAccess.Read);
                using (var reader = binder.Bind<TextReader>(inputBlob))
                {
                    if (reader != null)
                    {
                        return new OkObjectResult(reader.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"GetLogFunc got an exception: {ex.Message}");
            }

            // return empty response in case when file was removed from the blob storage or if exception was thrown 
            return new OkObjectResult("");
        }
    }
}
