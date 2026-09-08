using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ComputerComponents.Application.DTOs;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.API.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComputerComponents.Tests
{
    public class SpecificationsControllerTests
    {
        private readonly Mock<ISpecificationService> _specificationServiceMock;
        private readonly SpecificationsController _controller;

        public SpecificationsControllerTests()
        {
            _specificationServiceMock = new Mock<ISpecificationService>();
            _controller = new SpecificationsController(_specificationServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithSpecifications()
        {
            // Arrange
            var list = new List<SpecificationDto>
            {
                new SpecificationDto { Id = 1, PropertyName = "Cores", PropertyValue = "24", ComponentId = 1 }
            };
            _specificationServiceMock.Setup(s => s.GetAllAsync(null)).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var specs = Assert.IsAssignableFrom<IEnumerable<SpecificationDto>>(okResult.Value);
            Assert.Single(specs);
        }

        [Fact]
        public async Task Create_AddsSpecificationSuccessfully()
        {
            // Arrange
            var createDto = new CreateSpecificationDto { PropertyName = "VRAM", PropertyValue = "24GB GDDR6X", ComponentId = 1 };
            var createdDto = new SpecificationDto { Id = 1, PropertyName = "VRAM", PropertyValue = "24GB GDDR6X", ComponentId = 1 };
            _specificationServiceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var spec = Assert.IsType<SpecificationDto>(createdResult.Value);
            Assert.Equal("VRAM", spec.PropertyName);
        }
    }
}
