using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using Azure;
using TraderAzFunctions.Entities;
using TraderAzFunctions.OutputDtos;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using TraderAzFunctions.Extensions;

namespace TraderAzFunctions
{
    /// <summary>
    /// This Az Func returns list of all log entries from the ImportLog table for the requested time frame (in the local time). 
    /// </summary>
    public static class GetAllLogsFunc
    {
        [FunctionName("GetAllLogs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("ImportLog")] TableClient tableClient,
            ILogger log)
        {
            var result = new List<ImportLogOutputDto>();

            try
            {
                // input dates should represent local datetime (without timezone offset), ex. 2023-06-06 13:45:00  
                (bool isInnputValid, string errorMessage) = ParseAndValidateInputDates(req, out var dateFrom, out var dateTo);

                if (!isInnputValid)
                {
                    return new BadRequestErrorMessageResult(errorMessage);
                }

                AsyncPageable<ImportLog> queryResults = tableClient.QueryAsync<ImportLog>(filter:
                    x => x.Timestamp >= dateFrom
                      && x.Timestamp <= dateTo);

                await foreach (ImportLog entity in queryResults)
                {
                    // in the real application I recommned to use Automapper 
                    result.Add(new ImportLogOutputDto
                    {
                        Id = entity.RowKey,
                        IsSucceded = entity.IsSucceded,
                        Timestamp = entity.Timestamp.ConvertUTCToPolandLocalTime()
                });
                }

                // tableClient does not support OrderBy 
                return new OkObjectResult(result.OrderBy(x => x.Timestamp));
            }
            catch(Exception ex)
            {
                log.LogError($"GetAllLogsFunc got an exception: {ex.Message}");
                return new NoContentResult();
            }
        }

        private static (bool status, string errorMessage) ParseAndValidateInputDates(HttpRequest req, out DateTimeOffset dateFrom, out DateTimeOffset dateTo)
        {
            string from = req.Query["from"];
            string to = req.Query["to"];

            dateFrom = default;
            dateTo = default;

            // perform basic validation 
            if (string.IsNullOrEmpty(from) || !DateTime.TryParse(from, out var cvFromDate))
            {
                return (false, "\"from\" parameter has invalid format!");
            }
            if (string.IsNullOrEmpty(to) || !DateTime.TryParse(to, out var cvToDate))
            {
                return (false, "\"to\" parameter has invalid format!");
            }
            if (cvFromDate > cvToDate)
            {
                return (false, "\"from\" can't be greater than \"to\" parameter !");
            }

            // I have to make this time conversion, becuase application can be hosted on the different time zone.
            dateFrom = cvFromDate.ConvertPolandLocalTimeToUTC();
            dateTo = cvToDate.ConvertPolandLocalTimeToUTC();

            return (true, default);
        }
    }
}
