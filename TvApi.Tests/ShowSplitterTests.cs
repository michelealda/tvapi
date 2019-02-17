using System.Linq;
using FluentAssertions;
using TvApi.Infrastructure.Extensions;
using Xunit;

namespace TvApi.Tests
{
    public class ShowSplitterTests
    {
        [Fact]
        public void SplitWithCorrectSplitSize()
        {
            var splittedDictionary = Enumerable.Range(0, 8)
                  .Select(i => i)
                  .ToDictionary(kv => kv)
                  .SplitShows(5)
                  .ToArray();

            splittedDictionary
                .First().Should().HaveCount(5);

            splittedDictionary
                .Last().Should().HaveCount(3);
        }

        [Fact]
        public void SplitWithLargerSize()
            => Enumerable.Range(0, 5)
                .Select(i => i)
                .ToDictionary(kv => kv)
                .SplitShows(10)
                .Single()
                .Should()
                .HaveCount(5);
    }
}