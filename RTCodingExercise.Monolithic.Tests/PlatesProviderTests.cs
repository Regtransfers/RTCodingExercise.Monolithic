using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using NanoidDotNet;
using RTCodingExercise.Monolithic.Business;
using RTCodingExercise.Monolithic.Common;
using RTCodingExercise.Monolithic.Common.Models;
using RTCodingExercise.Monolithic.DataAccess;
using Xunit;

namespace RTCodingExercise.Monolithic.Tests
{
    public class PlatesProviderTests
    {
        private PlatesProvider _classInTest;
        private List<Plate> _platesList;
        private Mock<ILogger<PlatesProvider>> _logger;
        private Mock<DbSet<Plate>> _platesDbSet; 
        private ApplicationDbContext dbContext;

        public PlatesProviderTests()
        {
            _platesList = PlatesTestData.PlatesList;
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName:"plates");
            dbContext = new ApplicationDbContext(optionsBuilder.Options);
            
            _platesDbSet = _platesList.AsQueryable().BuildMockDbSet();
            var platesRepository = new Repository<Plate>(dbContext, _platesDbSet.Object);
            _logger = new Mock<ILogger<PlatesProvider>>();
            
            _classInTest = new PlatesProvider(platesRepository, _logger.Object);
        }

        private static string GenerateAndDeconstructRegistration(out string letters1, out int numbers, out string letters2)
        {
            letters1 = Nanoid.Generate(Nanoid.Alphabets.Letters, 1);
            numbers = int.Parse(Nanoid.Generate(Nanoid.Alphabets.Digits, 3));
            letters2 = Nanoid.Generate(Nanoid.Alphabets.Letters, 3);
            var reg = $"{letters1}{numbers}{letters2}";
            return reg;
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
            var actual = await _classInTest.GetAllAsync(pageIndex: pageIndex);

            var expected = 
                _platesList
                    .Skip((pageIndex * PlatesProvider.PAGE_SIZE) - PlatesProvider.PAGE_SIZE)
                    .Take(PlatesProvider.PAGE_SIZE);
            
            actual.Should().Contain(expected);
        }

        [Fact]
        public Task GetAllPlates_ForPageZero_ThrowsArgumentException()
        {
            var act = () => _classInTest.GetAllAsync(pageIndex: 0);
            
            act.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>().WithMessage($"pageIndex must be greater than zero");
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetAllPlates_Unsorted_ReturnsUnsortedPlates()
        {
            var expected = _platesList.Take(PlatesProvider.PAGE_SIZE);
            var actual = await _classInTest.GetAllAsync(pageIndex: 1);

            actual.Should().Contain(expected);
        }
        
        [Fact]
        public async Task GetAllPlates_SortedByPriceAsc_ReturnsSortedPlates()
        {
            var expected = _platesList.OrderBy(p => p.MarkUp).Take(PlatesProvider.PAGE_SIZE);
            var actual = await _classInTest.GetAllAsync(pageIndex: 1, sortOrder: SortOrderEnums.Ascending);

            actual.Should().Contain(expected);
        }
        
        [Fact]
        public async Task GetAllPlates_SortedByPriceDesc_ReturnsSortedPlates()
        {
            var expected = _platesList.OrderByDescending(p => p.MarkUp).Take(PlatesProvider.PAGE_SIZE);
            var actual = await _classInTest.GetAllAsync(pageIndex: 1, sortOrder: SortOrderEnums.Descending);

            actual.Should().Contain(expected);
        }



        [Theory]
        [MemberData(nameof(PlatesTestData.Data), MemberType= typeof(PlatesTestData))]
        public async Task GetAllPlates_WithNumberFilter_ReturnsExpectedPlates(Plate plate)
        {
            var value = plate.Numbers.ToString();
            var expected = _platesList.Where(x => x.Numbers.ToString().Contains(value));

            var actual = await _classInTest.GetAllAsync(searchFilter: value);

            actual.Should().Contain(expected);
        }
        
        [Theory]
        [MemberData(nameof(PlatesTestData.Data), MemberType= typeof(PlatesTestData))]
        public async Task GetAllPlates_WithLetterFilter_ReturnsExpectedPlates(Plate plate)
        {
            var value = plate.Letters;
            var expected = _platesList.Where(x => x.Letters.Contains(value));

            var actual = await _classInTest.GetAllAsync(searchFilter: value);

            actual.Should().Contain(expected);
        }
        
        [Fact]
        public async Task GetAllPlates_WithLetterFilterSecondPage_ReturnsExpectedPlates()
        {
            var value = PlatesTestData.PLATE_CONSTANT;
            var pageIndex = 2;
            
            var expected = 
                _platesList
                    .Where(x => x.Registration.Contains(value))
                    .Skip((pageIndex * PlatesProvider.PAGE_SIZE) - PlatesProvider.PAGE_SIZE)
                    .Take(PlatesProvider.PAGE_SIZE);
            
            var actual = await _classInTest.GetAllAsync(searchFilter: value, pageIndex: pageIndex);

            actual.Should().NotBeEmpty();
            actual.Should().AllSatisfy(x => x.Registration.Contains(value));
            actual.Should().Contain(expected);
        }
        
        [Fact]
        public void AddPlate_WithNumbersAndLetters_ExtractsNumbersAndLetters()
        {
            Plate actual = null;
            _platesDbSet.Setup(x => x.Add(It.IsAny<Plate>()))
                .Callback<Plate>((plate) => actual = plate);
            var reg = GenerateAndDeconstructRegistration(out var letters1, out var numbers, out var letters2);

            _classInTest.AddPlate(new Plate {Registration = reg});
            _platesDbSet.Verify(plate => plate.Add(It.IsAny<Plate>()), Times.Once);

            actual.Letters.Should().Be($"{letters1}{letters2}".ToUpper());
            actual.Numbers.Should().Be(numbers);
        }
        
        [Fact]
        public async Task AddPlate_WithNumbers_ExtractsOnlyNumbers()
        {
            Plate actual = null;
            _platesDbSet.Setup(x => x.Add(It.IsAny<Plate>()))
                .Callback<Plate>((plate) => actual = plate);
            var reg = GenerateAndDeconstructRegistration(out var letters1, out var numbers, out var letters2);
            reg = numbers.ToString();

            _classInTest.AddPlate(new Plate {Registration = reg});
            _platesDbSet.Verify(plate => plate.Add(It.IsAny<Plate>()), Times.Once);

            actual.Letters.Should().BeEmpty();
            actual.Numbers.Should().Be(numbers);
        }
        
        [Fact]
        public async Task AddPlate_WithLetters_ExtractsOnlyLetters()
        {
            Plate actual = null;
            _platesDbSet.Setup(x => x.Add(It.IsAny<Plate>()))
                .Callback<Plate>((plate) => actual = plate);
            var reg = GenerateAndDeconstructRegistration(out var letters1, out var numbers, out var letters2);
            reg = $"{letters1}{letters2}";

            _classInTest.AddPlate(new Plate {Registration = reg});
            _platesDbSet.Verify(plate => plate.Add(It.IsAny<Plate>()), Times.Once);

            actual.Letters.Should().Be(reg.ToUpper());
            actual.Numbers.Should().Be(0);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ToggleReservePlate_WithInitialStatus_ReservesPlate(bool initialStatus)
        {
            var plate = _platesList.First(x => x.Reserved == initialStatus);

            dbContext.Plates.Add(plate);
            await dbContext.SaveChangesAsync();
            _platesDbSet.Setup(x => x.Find(It.IsAny<object>()))
                .Returns(plate);

            Plate actual = null;
            _platesDbSet.Setup(x => x.Attach(It.IsAny<Plate>()))
                .Callback<Plate>((plate) => actual = plate);
            
            _classInTest.ToggleReserve(plate.Id);

            actual.Reserved.Should().Be(!initialStatus);

            dbContext.Plates.Remove(plate);
            await dbContext.SaveChangesAsync();
        }
        
        [Fact]
        public Task ReservePlate_UnknownPlate_Throws()
        {
            var plateId = new Guid();
            var act = () => _classInTest.ToggleReserve(plateId);
            act.Should().Throw<KeyNotFoundException>().WithMessage($"No plate exists with Id of: {plateId}");;
            return Task.CompletedTask;
        }
    }
}