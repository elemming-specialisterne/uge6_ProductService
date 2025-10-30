using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService.Models;
using ProductService.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Test.UnitTests
{
    public class ProductsController_PostTests
    {
        [Fact]
        public async Task Create_ValidProduct_ReturnsCreatedProduct()
        {
            // Arrange: mock repository to add a new product and return it
            var mockRepo = new Mock<IProductRepository>();
            var newProduct = new Product
            {
                Productid = 1,
                Name = "New Product",
                Description = "Description",
                Price = 50m,
            };
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(newProduct);

            var controller = new ProductsController(mockRepo.Object);

            // Act: call Create with valid product
            var result = await controller.Create(newProduct);

            // Assert: returns CreatedAtActionResult with created product
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedProduct = Assert.IsType<Product>(createdResult.Value);
            Assert.Equal(newProduct.Productid, returnedProduct.Productid);
            Assert.Equal("New Product", returnedProduct.Name);
        }

        [Fact]
        public async Task Create_InvalidProduct_ReturnsBadRequest()
        {
            // Arrange: simulate invalid model state
            var mockRepo = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepo.Object);
            controller.ModelState.AddModelError("Name", "Required"); // simulate invalid input

            var invalidProduct = new Product();

            // Act: call Create with invalid product
            var result = await controller.Create(invalidProduct);

            // Assert: should return BadRequest due to invalid model
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}