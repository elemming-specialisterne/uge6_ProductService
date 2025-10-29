using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService.Models;
using ProductService.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Test.UnitTests
{
    public class ProductsController_GetTests
    {
        [Fact]
        public async Task GetAll_ReturnsListOfProducts()
        {
            // Arrange: mock repository to return a list of products
            var mockRepo = new Mock<IProductRepository>();
            var testProducts = new List<Product>
            {
                new Product { ProductID = 1, Name = "Coffee", Category = "Drinks", Price = 25m },
                new Product { ProductID = 2, Name = "Tea", Category = "Drinks", Price = 15m }
            };
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(testProducts);

            var controller = new ProductsController(mockRepo.Object);

            // Act: call GetAll
            var result = await controller.GetAll();

            // Assert: returns OkObjectResult containing all products
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(2, products.Count());
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsProduct()
        {
            // Arrange: mock repository to return a specific product
            var mockRepo = new Mock<IProductRepository>();
            var product = new Product { ProductID = 1, Name = "Coffee", Category = "Drinks", Price = 25m };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var controller = new ProductsController(mockRepo.Object);

            // Act: call GetById
            var result = await controller.GetById(1);

            // Assert: returns OkObjectResult with the expected product
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.ProductID, returnedProduct.ProductID);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange: mock repository returns null for non-existing ID
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            var controller = new ProductsController(mockRepo.Object);

            // Act
            var result = await controller.GetById(999);

            // Assert: returns NotFoundResult for invalid product ID
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}