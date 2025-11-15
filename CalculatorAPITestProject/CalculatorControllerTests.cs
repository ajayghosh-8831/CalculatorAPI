using CalculatorAPI.Contracts;
using CalculatorAPI.Controllers;
using CalculatorAPI.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CalculatorAPITestProject
{

        public class CalculatorControllerTests
        {
            private readonly Mock<ICalculatorResource> _mockCalculatorResource;
            private readonly Mock<ILogger<CalculatorController>> _mockLogger;
            private readonly CalculatorController _controller;

            public CalculatorControllerTests()
            {
                _mockCalculatorResource = new Mock<ICalculatorResource>();
                _mockLogger = new Mock<ILogger<CalculatorController>>();
                _controller = new CalculatorController(_mockCalculatorResource.Object, _mockLogger.Object);
            }

          

            [Fact]
            public async Task Calculate_WithValidRequest_ReturnsOkResult()
            {
                // Arrange
                var request = new ProbabilityRequest
                {
                    PA = 0.5,
                    PB = 0.5,
                    Operation = "Either"
                };

                var expectedResult = new ProbabilityResult
                {
                    PA = 0.5,
                    PB = 0.5,
                    operation = "Either",
                    Result = 0.75
                };

                _mockCalculatorResource
                    .Setup(x => x.CreateAsync(request))
                    .ReturnsAsync(expectedResult);

                // Act
                var result = await _controller.Calculate(request);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal(expectedResult, okResult.Value);
                _mockCalculatorResource.Verify(x => x.CreateAsync(request), Times.Once);
            }

            [Fact]
            public async Task Calculate_CallsCalculatorResourceCreateAsync()
            {
                // Arrange
                var request = new ProbabilityRequest
                {
                    PA = 0.5,
                    PB = 0.5,
                    Operation = "CombinedWith"
                };

                _mockCalculatorResource
                    .Setup(x => x.CreateAsync(request))
                    .ReturnsAsync(new ProbabilityResult { Result = 0.25 });

                // Act
                await _controller.Calculate(request);

                // Assert
                _mockCalculatorResource.Verify(
                    x => x.CreateAsync(It.IsAny<ProbabilityRequest>()),
                    Times.Once);
            }
        }
    

}
