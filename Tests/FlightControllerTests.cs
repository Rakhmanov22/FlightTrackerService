using FlightStatusControlAPI.Controllers;
using FlightStatusControlAPI.Interfaces;
using FlightStatusControlAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace FlightStatusControlAPI.Tests
{
    public class FlightControllerTests
    {
        [Fact]
        public void GetFlights_ReturnsOkResult()
        {
            // Arrange
            var mockRepository = new Mock<IFlightRepository>();
            var mockCache = new Mock<IMemoryCache>();
            var mockLogger = new Mock<ILogger<FlightController>>();

            var controller = new FlightController(mockRepository.Object, mockCache.Object, mockLogger.Object);

            // Act
            var result = controller.GetFlights();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var flights = Assert.IsType<List<Flight>>(okResult.Value);
            Assert.Empty(flights);
        }

        [Fact]
        public void AddFlight_ReturnsOkResult()
        {
            // Arrange
            var mockRepository = new Mock<IFlightRepository>();
            var mockCache = new Mock<IMemoryCache>();
            var mockLogger = new Mock<ILogger<FlightController>>();

            var controller = new FlightController(mockRepository.Object, mockCache.Object, mockLogger.Object);

            var newFlight = new Flight
            {
                Origin = "TestOrigin",
                Destination = "TestDestination",
                Departure = DateTime.UtcNow,
                Arrival = DateTime.UtcNow.AddHours(1),
                Status = FlightStatus.InTime
            };

            // Act
            var result = controller.AddFlight(newFlight);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Flight added successfully", okResult.Value);
            mockRepository.Verify(repo => repo.AddFlight(It.IsAny<Flight>()), Times.Once);
        }
    }
}
