using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Models;
using Xunit;

namespace ProductService.Test
{
    public class ProductsController_PutTests
    {
        // Test: PUT update existing product
        [Fact]
        public void Update_ExistingProduct_UpdatesValuesCorrectly()
        {
            // Arrange: create controller and add a product
            var controller = new ProductsController();
            var product = new Product { Name = "Kaffe", Description = "Sort kaffe", Price = 25.50m, Inventory = 100, Category = "Drikkevarer" };
            var createResult = controller.Create(product);
            var createdProduct = Assert.IsType<Product>(((CreatedAtActionResult)createResult.Result).Value);

            var updatedProduct = new Product
            {
                Id = createdProduct.Id,
                Name = "Kaffe Opdateret",
                Description = "Ny beskrivelse",
                Price = 30.00m,
                Inventory = 80,
                Category = "Drikkevarer",
                Active = true
            };

            // Act: call Update()
            var updateResult = controller.Update(createdProduct.Id, updatedProduct);

            // Assert: returns NoContentResult
            Assert.IsType<NoContentResult>(updateResult);

            // Assert: check that product values are updated
            var getResult = controller.GetById(createdProduct.Id);
            var okResult = Assert.IsType<OkObjectResult>(getResult.Result);
            var productFromController = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("Kaffe Opdateret", productFromController.Name);
            Assert.Equal("Ny beskrivelse", productFromController.Description);
            Assert.Equal(30.00m, productFromController.Price);
            Assert.Equal(80, productFromController.Inventory);
        }

        // Test: PUT update non-existing product
        [Fact]
        public void Update_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange: create controller and a fake product
            var controller = new ProductsController();
            var nonExistingProduct = new Product { Id = 999, Name = "Fiktiv", Description = "Findes ikke", Price = 99.99m };

            // Act: call Update() with non-existing Id
            var result = controller.Update(999, nonExistingProduct);

            // Assert: returns NotFoundResult
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
