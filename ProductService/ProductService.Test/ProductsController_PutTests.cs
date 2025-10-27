using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Models;
using Xunit;

namespace ProductService.Test
{
    public class ProductsController_PutTests
    {
        // Test: Put update existing product
        [Fact]
        public void Update_ExistingProduct_UpdatesValuesCorrectly()
        {
            // Arrange: create controller and add a product
            var controller = new ProductsController();
            var product = new Product { Name = "Coffee", Description = "Black coffee", Price = 25.50m, Inventory = 100, Category = "Drinks" };
            var createResult = controller.Create(product);
            var createdProduct = Assert.IsType<Product>(((CreatedAtActionResult)createResult.Result).Value);

            var updatedProduct = new Product
            {
                ProductID = createdProduct.ProductID,
                Name = "Coffee updated",
                Description = "New description",
                Price = 30.00m,
                Inventory = 80,
                Category = "Drinks",
                Active = true
            };

            // Act: call Update()
            var updateResult = controller.Update(createdProduct.ProductID, updatedProduct);

            // Assert: returns NoContentResult
            Assert.IsType<NoContentResult>(updateResult);

            // Assert: check that product values are updated
            var getResult = controller.GetById(createdProduct.ProductID);
            var okResult = Assert.IsType<OkObjectResult>(getResult.Result);
            var productFromController = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("Coffee updated", productFromController.Name);
            Assert.Equal("New description", productFromController.Description);
            Assert.Equal(30.00m, productFromController.Price);
            Assert.Equal(80, productFromController.Inventory);
        }

        // Test: Put update non-existing product
        [Fact]
        public void Update_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange: create controller and a fake product
            var controller = new ProductsController();
            var nonExistingProduct = new Product { ProductID = 999, Name = "Fiction", Description = "Not found", Price = 99.99m };

            // Act: call Update() with non-existing Id
            var result = controller.Update(999, nonExistingProduct);

            // Assert: returns NotFoundResult
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
