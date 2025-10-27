using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Models;
using Xunit;

namespace ProductService.Test
{
    public class ProductsController_PostTests
    {
        // Test: Post create product
        [Fact]
        public void Create_ValidProduct_ReturnsCreatedProduct()
        {
            // Arrange: create controller and new product
            var controller = new ProductsController();
            var newProduct = new Product { Name = "New Product", Description = "Description", Price = 50m, Inventory = 20, Category = "Test" };

            // Act: call Create()
            var result = controller.Create(newProduct);

            // Assert: result is CreatedAtActionResult and product has Id assigned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdProduct = Assert.IsType<Product>(createdResult.Value);
            Assert.True(createdProduct.ProductID > 0);
            Assert.Equal("New Product", createdProduct.Name);
        }
    }
}
