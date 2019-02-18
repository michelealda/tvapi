using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TvApi.Core;
using TvApi.Infrastructure;
using Xunit;
using Mocks = TvApi.Tests.Internals.Mocks;

namespace TvApi.Tests
{
    public class LocalShowProviderTests
    {
        private readonly Mock<IShowRepository> _repositoryMock;
        public LocalShowProviderTests()
        {
            _repositoryMock = Mocks.GetShowRepository();
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