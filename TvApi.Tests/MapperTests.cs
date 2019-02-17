using System.Linq;
using Bogus;
using FluentAssertions;
using TvApi.Infrastructure.Dto;
using TvApi.Infrastructure.Mappers;
using Xunit;

namespace TvApi.Tests
{
    public class MapperTests
    {
        [Fact]
        public void CastShouldBeOrderedByBirthday()
        {
            var personFaker = new Faker<PersonDto>()
                .RuleFor(o => o.Id, f => f.Random.Int(min: 1))
                .RuleFor(o => o.Name, f => f.Name.FirstName())
                .RuleFor(o => o.Birthday, f => f.Date.Past());
                
            var castFaker  = new Faker<CastDto>()
                .RuleFor(o => o.Person, f => personFaker.Generate(1).Single());

            var dto = new Faker<TvShowDto>()
                    //OrderId is deterministic
                    .RuleFor(o => o.Id, f => f.Random.Int(min:1))
                    //Pick some fruit from a basket
                    .RuleFor(o => o.Name, f => f.Company.CompanyName())
                    //A random quantity from 1 to 10
                    .RuleFor(o => o._embedded, f => new EmbeddedDto() {Cast = castFaker.Generate(5)})
                .Generate(1).First();

            dto.MapToShow().Cast.Should().BeInAscendingOrder(c => c.Birthday);
        }
    }
}
