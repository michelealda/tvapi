using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using TvApi.Core;
using TvApi.Infrastructure;
using TvApi.Models;
using Xunit;

namespace TvApi.Tests
{
    public class LocalShowProviderTests
    {
        private readonly Mock<IShowRepository> _repositoryMock;
        public LocalShowProviderTests()
        {
            var castFaker = new Faker<Models.Person>()
                .RuleFor(o => o.Id, f => f.Random.Int(min: 1))
                .RuleFor(o => o.Name, f => f.Name.FirstName())
                .RuleFor(o => o.Birthday, f => f.Date.Past());
            var showFaker = new Faker<Show>()
                .RuleFor(o => o.Id, f => f.Random.Int(min: 1))
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(o => o.Cast, f => castFaker.Generate(3).ToArray());

            _repositoryMock = new Mock<IShowRepository>();
            _repositoryMock.Setup(x => x.GetAll())
                .Returns(showFaker.Generate(20));
        }

        [Fact]
        public async Task ShowsShouldBePaged()
        {
            const int itemsPerPage = 5;
            (await new LocalShowProvider(_repositoryMock.Object)
                    .GetPagedShows(0, itemsPerPage))
                .Should()
                .HaveCount(itemsPerPage);
        }

        [Fact]
        public async Task OutOfRangePageShouldReturnEmptyList()
        {
            (await new LocalShowProvider(_repositoryMock.Object)
                    .GetPagedShows(100, 10))
                .Should()
                .BeEmpty();
        }
    }
}