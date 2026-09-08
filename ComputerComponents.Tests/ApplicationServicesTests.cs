using Xunit;
using Moq;
using ComputerComponents.Application.DTOs;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Application.Services;
using ComputerComponents.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComputerComponents.Tests
{
    public class ApplicationServicesTests
    {
        [Fact]
        public async Task CategoryService_GetAllAsync_ReturnsMappedDtos()
        {
            // Arrange
            var repoMock = new Mock<ICategoryRepository>();
            var categories = new List<Category>
            {
                new Category { Id = 1, CategoryName = "RAM", Description = "Memory" }
            };
            repoMock.Setup(r => r.GetCategoriesWithComponentsAsync()).ReturnsAsync(categories);

            var service = new CategoryService(repoMock.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task UserService_AuthenticateAsync_ReturnsUserDto_WhenValid()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var user = new User
            {
                Id = 1,
                FullName = "Admin",
                Email = "test@system.com",
                Password = "password",
                Role = "Admin",
                Status = "نشط"
            };
            repoMock.Setup(r => r.AuthenticateAsync("test@system.com", "password")).ReturnsAsync(user);

            var service = new UserService(repoMock.Object);

            // Act
            var result = await service.AuthenticateAsync(new LoginRequestDto
            {
                Email = "test@system.com",
                Password = "password"
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Admin", result.FullName);
            Assert.Equal("test@system.com", result.Email);
        }

        [Fact]
        public async Task DashboardService_GetStatsAsync_ReturnsCalculatedStats()
        {
            // Arrange
            var compRepoMock = new Mock<IComponentRepository>();
            var catRepoMock = new Mock<ICategoryRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var specRepoMock = new Mock<ISpecificationRepository>();

            compRepoMock.Setup(r => r.GetCountAsync()).ReturnsAsync(10);
            catRepoMock.Setup(r => r.GetCountAsync()).ReturnsAsync(4);
            userRepoMock.Setup(r => r.GetCountAsync()).ReturnsAsync(5);
            specRepoMock.Setup(r => r.GetCountAsync()).ReturnsAsync(20);
            compRepoMock.Setup(r => r.GetTotalInventoryValueAsync()).ReturnsAsync(15000m);
            compRepoMock.Setup(r => r.GetRecentComponentsAsync(5)).ReturnsAsync(new List<Component>());
            catRepoMock.Setup(r => r.GetCategoriesWithComponentsAsync()).ReturnsAsync(new List<Category>());

            var service = new DashboardService(compRepoMock.Object, catRepoMock.Object, userRepoMock.Object, specRepoMock.Object);

            // Act
            var result = await service.GetStatsAsync();

            // Assert
            Assert.Equal(10, result.TotalComponents);
            Assert.Equal(4, result.TotalCategories);
            Assert.Equal(5, result.TotalUsers);
            Assert.Equal(20, result.TotalSpecifications);
            Assert.Equal(15000m, result.TotalInventoryValue);
        }
    }
}
