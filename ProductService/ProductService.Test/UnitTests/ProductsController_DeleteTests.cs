using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Test.UnitTests
{
    public class ProductsController_DeleteTests
    {
        [Fact]
        public async Task Delete_ExistingProduct_ReturnsNoContent()
        {
            // Arrange: mock the repository and setup DeleteAsync to complete successfully
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask).Verifiable();

            var controller = new ProductsController(mockRepo.Object);

            // Act: call Delete method
            var result = await controller.Delete(1);

            // Assert: should return NoContentResult and repository method called once
            Assert.IsType<NoContentResult>(result);
            mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingProduct_ReturnsNoContent()
        {
            // Arrange: simulate deleting a non-existing product
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.DeleteAsync(999)).Returns(Task.CompletedTask).Verifiable();

            var controller = new ProductsController(mockRepo.Object);

            // Act
            var result = await controller.Delete(999);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockRepo.Verify(r => r.DeleteAsync(999), Times.Once);
        }
    }
}