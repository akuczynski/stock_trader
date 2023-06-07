using System; 

namespace TraderAzFunctions.Extensions
{
    public static  class DateTimeExtensionMethods
    {
        private const string PolandTimeZoneName = "Central European Standard Time";

        public static DateTime ConvertUTCToPolandLocalTime(this DateTimeOffset? dateTime)
        {
            TimeZoneInfo myTimeZone = TimeZoneInfo.FindSystemTimeZoneById(PolandTimeZoneName);
            var utcTimeStamp = ((DateTimeOffset)dateTime).ToUniversalTime().DateTime;

            return TimeZoneInfo.ConvertTimeFromUtc(utcTimeStamp, myTimeZone);
        }

        public static DateTimeOffset ConvertPolandLocalTimeToUTC(this DateTime dateTime)
        {
            TimeZoneInfo myTimeZone = TimeZoneInfo.FindSystemTimeZoneById(PolandTimeZoneName);
            DateTimeOffset getDate = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, myTimeZone);
            string TimeZoneId = " " + getDate.ToString("zzz");

            DateTimeOffset.TryParse(dateTime + TimeZoneId, out var cvDate);
            return cvDate.ToUniversalTime(); 
        }

    }
}
