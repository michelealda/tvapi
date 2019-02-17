using Newtonsoft.Json.Converters;

namespace TvApi.Models.Converters
{
    public class BirthdayDateTimeConverter : IsoDateTimeConverter
    {
        public BirthdayDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}