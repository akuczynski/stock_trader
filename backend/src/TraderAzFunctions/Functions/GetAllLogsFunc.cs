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
using System.Globalization;
using System.Linq;

namespace TraderAzFunctions
{
    /// <summary>
    /// This Az Func returns list of all log entries from the ImportLog table for the requested time frame (in local time). 
    /// </summary>
    public static class GetAllLogsFunc
    {
        private const string PolandTimeZoneName = "Central European Standard Time";

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
                    // convert utc time to the local time in Poland  
                    TimeZoneInfo myTimeZone = TimeZoneInfo.FindSystemTimeZoneById(PolandTimeZoneName);
                    var utcTimeStamp = ((DateTimeOffset)entity.Timestamp).ToUniversalTime().DateTime;

                    var locaTime = TimeZoneInfo.ConvertTimeFromUtc(utcTimeStamp, myTimeZone);

                    // in the real application I recommned to use Automapper 
                    result.Add(new ImportLogOutputDto
                    {
                        Id = entity.RowKey,
                        IsSucceded = entity.IsSucceded,
                        Timestamp = locaTime
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

            // I have to make this time conversion, becuase code can be hosted in the different time zone where input dates are in the Warsaw time zone. 
            TimeZoneInfo myTimeZone = TimeZoneInfo.FindSystemTimeZoneById(PolandTimeZoneName);
            DateTimeOffset getDate = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, myTimeZone);
            string TimeZoneId = " " + getDate.ToString("zzz");

            dateFrom = default;
            dateTo = default;

            // perform basic validation 
            if (string.IsNullOrEmpty(from) || !DateTimeOffset.TryParse(from + TimeZoneId, out var cvFromDate))
            {
                return (false, "\"from\" parameter has invalid format!");
            }
            if (string.IsNullOrEmpty(to) || !DateTimeOffset.TryParse(to + TimeZoneId, out var cvToDate))
            {
                return (false, "\"to\" parameter has invalid format!");
            }
            if (cvFromDate > cvToDate)
            {
                return (false, "\"from\" can't be greater than \"to\" parameter !");
            }

            dateFrom = cvFromDate.ToUniversalTime();
            dateTo = cvToDate.ToUniversalTime();

            return (true, default);
        }
    }
}
