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
    public static class GetLogFunc
    {
        [FunctionName("GetLog")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            IBinder binder,
            ILogger log)
        {
            string inputId = req.Query["id"];
            if (string.IsNullOrEmpty(inputId) || !Guid.TryParse(inputId, out var blobId))
            {
                return new BadRequestErrorMessageResult("\"id\" has invalid format");
            }

            var inputBlob = new BlobAttribute($"imported-data/{blobId}.json", FileAccess.Read);
            using (var reader = binder.Bind<TextReader>(inputBlob))
            {
                return new OkObjectResult(reader.ReadToEnd());
            }
        }
    }
}
