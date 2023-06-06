using Azure;
using Azure.Data.Tables;
using System;

namespace TraderAzFunctions.Entities
{
    public class ImportLog : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; } = default!;
        public bool IsSucceded { get; set; }
        public ETag ETag { get; set; }
    }
}
