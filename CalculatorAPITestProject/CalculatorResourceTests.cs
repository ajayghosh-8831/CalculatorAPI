using CalculatorAPI.Contracts;
using CalculatorAPI.Helper;
using CalculatorAPI.Resources;
using Xunit;

namespace CalculatorAPITestProject
{
    public class CalculatorResourceTests
    {
        private readonly CalculatorResource _calculatorResource;

        public CalculatorResourceTests()
        {
            _calculatorResource = new CalculatorResource();
        }

        [Theory]
        [InlineData(0.5, 0.5, "CombinedWith", 0.25)]
        [InlineData(0.3, 0.7, "CombinedWith", 0.21)]
        [InlineData(0.0, 1.0, "CombinedWith", 0.0)]
        [InlineData(1.0, 1.0, "CombinedWith", 1.0)]
        public async Task CreateAsync_CombinedWith_CalculatesCorrectly(double pa, double pb, string operation, double expected)
        {
            // Arrange
            var request = new ProbabilityRequest
            {
                PA = pa,
                PB = pb,
                Operation = operation
            };

            // Act
            var result = await _calculatorResource.CreateAsync(request);

            // Assert
            Assert.Equal(expected, result.Result);
            Assert.Equal(pa, result.PA);
            Assert.Equal(pb, result.PB);
            Assert.Equal(operation, result.operation);
        }

        [Theory]
        [InlineData(0.5, 0.5, "Either", 0.75)]
        [InlineData(0.3, 0.7, "Either", 0.79)]
        [InlineData(0.0, 0.0, "Either", 0.0)]
        [InlineData(1.0, 1.0, "Either", 1.0)]
        public async Task CreateAsync_Either_CalculatesCorrectly(double pa, double pb, string operation, double expected)
        {
            // Arrange
            var request = new ProbabilityRequest
            {
                PA = pa,
                PB = pb,
                Operation = operation
            };

            // Act
            var result = await _calculatorResource.CreateAsync(request);

            // Assert
            Assert.Equal(expected, result.Result);
            Assert.Equal(pa, result.PA);
            Assert.Equal(pb, result.PB);
            Assert.Equal(operation, result.operation);
        }

        [Fact]
        public async Task CreateAsync_InvalidOperation_ThrowsException()
        {
            // Arrange
            var request = new ProbabilityRequest
            {
                PA = 0.5,
                PB = 0.5,
                Operation = "InvalidOperation"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _calculatorResource.CreateAsync(request));
        }

        [Fact]
        public async Task CreateAsync_ReturnsCorrectType()
        {
            // Arrange
            var request = new ProbabilityRequest
            {
                PA = 0.5,
                PB = 0.5,
                Operation = "Either"
            };

            // Act
            var result = await _calculatorResource.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProbabilityResult>(result);
        }
    }
}
