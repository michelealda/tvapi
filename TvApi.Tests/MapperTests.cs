using System.Linq;
using FluentAssertions;
using TvApi.Infrastructure.Mappers;
using TvApi.Tests.Internals;
using Xunit;

namespace TvApi.Tests
{
    public class MapperTests
    {
        [Fact]
        public void CastShouldBeOrderedByBirthday()
            => Fakes
                .GenerateFakeTvShowDtos()
                .Single()
                .MapToShow()
                .Cast
                .Should()
                .BeInAscendingOrder(c => c.Birthday);

    }
}
