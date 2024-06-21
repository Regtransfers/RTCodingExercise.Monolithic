using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RTCodingExercise.Monolithic.Business;
using RTCodingExercise.Monolithic.Common.Models;
using RTCodingExercise.Monolithic.DataAccess.Interfaces;
using Xunit;

namespace RTCodingExercise.Monolithic.Tests
{
    public class PlatesProviderTests
    {
        private PlatesProvider _classInTest;
        private Mock<IRepository<Plate>> _plateRepository;

        public PlatesProviderTests()
        {
            _plateRepository = new Mock<IRepository<Plate>>();
            _classInTest = new PlatesProvider(_plateRepository.Object);
        }

        [Fact]
        public async Task GetAllPlates_ReturnsMarkup()
        {
            var list = new List<Plate>
            {
                new Plate
                {
                    SalePrice = 12,
                    MarkUp = new decimal(12 * 1.2)
                },
                new Plate
                {
                    SalePrice = 500,
                    MarkUp = new decimal(500 * 1.2)
                }
            };
            
            _plateRepository.Setup(x => x.Get(
                It.IsAny<Expression<Func<Plate, bool>>>(),
                null,
                It.IsAny<string>()))
                .Returns(list.AsQueryable());
            
            var actual = await _classInTest.GetAllAsync();
            
            actual.Should().AllSatisfy(plate => plate.MarkUp.Should().Be(plate.SalePrice * (decimal)1.2));
        }
    }
}