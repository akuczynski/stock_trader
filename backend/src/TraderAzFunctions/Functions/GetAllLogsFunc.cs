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
    public static class GetAllLogsFunc
    {
        [FunctionName("GetAllLogs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("ImportLog")] TableClient tableClient,
            ILogger log)
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

            var result = new List<ImportLogOutputDto>();

            await foreach (ImportLog entity in queryResults)
            {
                // convert utc time to local time 
                var localTime = ((DateTimeOffset)entity.Timestamp).ToLocalTime();

                // in the real application I recommned to use Automapper 
                result.Add(new ImportLogOutputDto
                {
                    Id = entity.RowKey,
                    IsSucceded = entity.IsSucceded,
                    Timestamp = localTime.DateTime
                });
            }

            // tableClient does not support OrderBy 
            return new OkObjectResult(result.OrderBy(x => x.Timestamp));
        }

        private static (bool status, string errorMessage) ParseAndValidateInputDates(HttpRequest req, out DateTimeOffset dateFrom, out DateTimeOffset dateTo)
        {
            string from = req.Query["from"];
            string to = req.Query["to"];

            dateFrom = default;
            dateTo = default;

            // perform basic validation 
            if (string.IsNullOrEmpty(from) || !DateTimeOffset.TryParse(from, null as IFormatProvider, DateTimeStyles.AdjustToUniversal, out dateFrom))
            {
                return (false, "\"from\" parameter has invalid format!");
            }
            if (string.IsNullOrEmpty(to) || !DateTimeOffset.TryParse(to, null as IFormatProvider, DateTimeStyles.AdjustToUniversal, out dateTo))
            {
                return (false, "\"to\" parameter has invalid format!");
            }
            if (dateFrom > dateTo)
            {
                return (false, "\"from\" can't be greater than \"to\" parameter !");
            }

            return (true, default);
        }
    }
}
