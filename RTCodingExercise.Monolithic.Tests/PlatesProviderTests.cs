using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MockQueryable.Moq;
using RTCodingExercise.Monolithic.Business;
using RTCodingExercise.Monolithic.Common.Models;
using RTCodingExercise.Monolithic.DataAccess;
using Xunit;

namespace RTCodingExercise.Monolithic.Tests
{
    public class PlatesProviderTests
    {
        private PlatesProvider _classInTest;
        private List<Plate> _plates = new List<Plate>();

        public PlatesProviderTests()
        {
            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                var salePrice = random.Next(1, 1000);
                _plates.Add(new Plate
                {
                    Registration = Guid.NewGuid().ToString(),
                    PurchasePrice = random.Next(1, 1000),
                    SalePrice = salePrice,
                    MarkUp = (decimal)(salePrice * 1.2)
                });    
            }

            var platesMock = _plates.AsQueryable().BuildMockDbSet();
            var platesRepository = new Repository<Plate>(platesMock.Object);
            _classInTest = new PlatesProvider(platesRepository);
        }

        [Fact]
        public async Task GetAllPlates_ReturnsMarkup()
        {
            var actual = await _classInTest.GetAllAsync();
            
            actual.Should().AllSatisfy(plate => plate.MarkUp.Should().Be(plate.SalePrice * (decimal)1.2));
        }
        
        [Theory]
        [ClassData(typeof(PaginationTestData))]
        public async Task GetAllPlates_ForSelectedPage_ReturnsCorrectPage(int pageIndex)
        {
            var actual = await _classInTest.GetAllAsync(pageIndex);

            var expected = _plates.GetRange((pageIndex * PlatesProvider.PAGE_SIZE) - PlatesProvider.PAGE_SIZE, PlatesProvider.PAGE_SIZE);

            actual.Should().Contain(expected);
        }

        [Fact]
        public Task GetAllPlates_ForPageZero_ThrowsArgumentException()
        {
            var act = () => _classInTest.GetAllAsync(0);
            
            act.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>().WithMessage($"pageIndex must be greater than zero");
            return Task.CompletedTask;
        }
    }

    public class PaginationTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            for (var i = 1; i <= 5; i++)
            {
                yield return new object[] { i };
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}