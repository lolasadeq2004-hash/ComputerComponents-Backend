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
    public class ComponentsControllerTests
    {
        private readonly Mock<IComponentService> _componentServiceMock;
        private readonly ComponentsController _controller;

        public ComponentsControllerTests()
        {
            _componentServiceMock = new Mock<IComponentService>();
            _controller = new ComponentsController(_componentServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithComponents()
        {
            // Arrange
            var list = new List<ComponentDto>
            {
                new ComponentDto { Id = 1, ComponentName = "Core i9", Model = "14900K", Price = 599.99m, ImageUrl = "i9.jpg", CategoryId = 1 }
            };
            _componentServiceMock.Setup(s => s.GetAllAsync(null, null)).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var components = Assert.IsAssignableFrom<IEnumerable<ComponentDto>>(okResult.Value);
            Assert.Single(components);
        }

        [Fact]
        public async Task Create_AddsComponentSuccessfully()
        {
            // Arrange
            var createDto = new CreateComponentDto { ComponentName = "RTX 4090", Model = "ROG Strix", Price = 1999.99m, ImageUrl = "rtx4090.jpg", CategoryId = 1 };
            var createdDto = new ComponentDto { Id = 1, ComponentName = "RTX 4090", Model = "ROG Strix", Price = 1999.99m, ImageUrl = "rtx4090.jpg", CategoryId = 1 };
            _componentServiceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var comp = Assert.IsType<ComponentDto>(createdResult.Value);
            Assert.Equal("RTX 4090", comp.ComponentName);
        }
    }
}
