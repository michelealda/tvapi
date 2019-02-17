using System.Collections.Generic;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TvApi.Controllers;
using TvApi.Core;
using TvApi.Models;
using Xunit;

namespace TvApi.Tests
{
    public class ControllerTests
    {
        private readonly Mock<IShowRepository> _repositoryMock;
        public ControllerTests()
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
        public void ShowsShouldBePaged()
        {
            const int itemsPerPage = 5;
            var result = new ShowsController(_repositoryMock.Object)
                .Get(0, itemsPerPage).Result as OkObjectResult;
            
            result
                .Should()
                .NotBeNull();

            (result?.Value as IEnumerable<Show>)
                .Should()
                .HaveCount(itemsPerPage);
        }

        [Fact]
        public void OutOfRangePageShouldReturnNotFound()
        {
            var result = new ShowsController(_repositoryMock.Object)
                .Get(10).Result;
                
            result.Should().BeAssignableTo<NotFoundResult>();
        }
    }
}