using Moq;
using TvApi.Core;

namespace TvApi.Tests.Internals
{
    public static class Mocks
    {
        public static Mock<IShowRepository> GetShowRepository(int itemsCount = 20)
        {
            var mock = new Mock<IShowRepository>();
            mock.Setup(x => x.GetAll())
                .Returns(Fakes.GenerateFakeShows(itemsCount));
            return mock;
        }
    }
}