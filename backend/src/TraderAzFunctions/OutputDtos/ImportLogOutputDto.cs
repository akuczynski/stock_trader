using System;

namespace TraderAzFunctions.OutputDtos
{
    internal class ImportLogOutputDto
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSucceded { get; set; }
    }
}
