using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService.Models;
using ProductService.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Test.UnitTests
{
    public class ProductsController_PutTests
    {
        [Fact]
        public async Task Update_ExistingProduct_ReturnsNoContent()
        {
            // Arrange: mock repository for updating existing product
            var mockRepo = new Mock<IProductRepository>();
            var updatedProduct = new Product
            {
                Productid = 1,
                Name = "Updated Coffee",
                Description = "Updated description",
                Price = 30m
            };
            mockRepo.Setup(r => r.UpdateAsync(updatedProduct)).Returns(Task.CompletedTask);

            var controller = new ProductsController(mockRepo.Object);

            // Act: call Update with correct product ID
            var result = await controller.Update(1, updatedProduct);

            // Assert: should return NoContentResult
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NonExistingProduct_ReturnsBadRequest()
        {
            // Arrange: simulate updating a product with mismatched ID
            var mockRepo = new Mock<IProductRepository>();
            var updatedProduct = new Product
            {
                Productid = 2,
                Name = "Coffee",
                Description = "Test",
                Price = 25m
            };

            var controller = new ProductsController(mockRepo.Object);

            // Act: attempt to update with mismatched ID
            var result = await controller.Update(1, updatedProduct);

            // Assert: should return BadRequestResult due to ID mismatch
            Assert.IsType<BadRequestResult>(result);
        }
    }
}