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
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoriesController(_categoryServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfCategories()
        {
            // Arrange
            var list = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, CategoryName = "Processors", Description = "CPUs" }
            };
            _categoryServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var categories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(okResult.Value);
            Assert.Single(categories);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenCategoryExists()
        {
            // Arrange
            var cat = new CategoryDto { Id = 1, CategoryName = "GPUs", Description = "Graphics Cards" };
            _categoryServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(cat);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var category = Assert.IsType<CategoryDto>(okResult.Value);
            Assert.Equal("GPUs", category.CategoryName);
        }

        [Fact]
        public async Task Create_AddsCategoryAndReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateCategoryDto { CategoryName = "RAM", Description = "Memory modules" };
            var createdDto = new CategoryDto { Id = 1, CategoryName = "RAM", Description = "Memory modules" };
            _categoryServiceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var category = Assert.IsType<CategoryDto>(createdResult.Value);
            Assert.Equal("RAM", category.CategoryName);
        }

        [Fact]
        public async Task Delete_RemovesCategory_WhenExists()
        {
            // Arrange
            _categoryServiceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCategory(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
