using System.Collections.Generic;
using System.Linq;
using Bogus;
using TvApi.Infrastructure.Dto;
using TvApi.Models;

namespace TvApi.Tests.Internals
{
    public static class Fakes
    {
        public static IEnumerable<Show> GenerateFakeShows(int count = 20)
        {
            var castFaker = new Faker<Models.Person>()
                .RuleFor(o => o.Id, f => f.Random.Int(min: 1))
                .RuleFor(o => o.Name, f => f.Name.FirstName())
                .RuleFor(o => o.Birthday, f => f.Date.Past());

            return new Faker<Show>()
                .RuleFor(o => o.Id, f => f.Random.Int(min: 1))
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(o => o.Cast, f => castFaker.Generate(3).ToArray())
                .Generate(count);
        }

        public static IEnumerable<TvShowDto> GenerateFakeTvShowDtos(int count = 1, int castCount =5)
        {
            var personFaker = new Faker<PersonDto>()
                .RuleFor(o => o.Id, f => f.Random.Int(min: 1))
                .RuleFor(o => o.Name, f => f.Name.FirstName())
                .RuleFor(o => o.Birthday, f => f.Date.Past());

            var castFaker = new Faker<CastDto>()
                .RuleFor(o => o.Person, f => personFaker
                    .Generate(1)
                    .Single());

            return new Faker<TvShowDto>()
                .RuleFor(o => o.Id, f => f.Random.Int(min: 1))
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(o => o._embedded, f => new EmbeddedDto
                {
                    Cast = castFaker.Generate(castCount)
                })
                .Generate(count);
        }
    }
}