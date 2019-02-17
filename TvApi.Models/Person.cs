using System;
using Newtonsoft.Json;
using TvApi.Models.Converters;

namespace TvApi.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(BirthdayDateTimeConverter))]
        public DateTime? Birthday { get; set; }
    }
}