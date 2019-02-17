namespace TvApi.Infrastructure.Dto
{
    public class TvShowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EmbeddedDto _embedded { get; set; }
    }
}
