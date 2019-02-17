using LiteDB;

namespace TvApi.Models
{
    public class Show
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public Person[] Cast { get; set; }
    }
}
