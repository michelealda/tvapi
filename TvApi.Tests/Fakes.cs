using Bogus;
using Moq;
using TvApi.Core;
using TvApi.Models;

namespace TvApi.Tests
{
    public static class Fakes
    {
        public static Mock<IShowRepository> GetShowRepositoryMock(int itemsCount = 20)
        {
            var castFaker = new Faker<Models.Person>()
                .RuleFor(o => o.Id, f => f.Random.Int(min: 1))
                .RuleFor(o => o.Name, f => f.Name.FirstName())
                .RuleFor(o => o.Birthday, f => f.Date.Past());
            var showFaker = new Faker<Show>()
                .RuleFor(o => o.Id, f => f.Random.Int(min: 1))
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(o => o.Cast, f => castFaker.Generate(3).ToArray());

            var mock = new Mock<IShowRepository>();
            mock.Setup(x => x.GetAll())
                .Returns(showFaker.Generate(itemsCount));
            return mock;
        }
    }
}