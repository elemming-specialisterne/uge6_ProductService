using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Models;
using Xunit;
using System.Collections.Generic;

namespace ProductService.Test
{
    public class ProductsController_GetTests
    {
        // Test: Get all products
        [Fact]
        public void GetAll_ReturnsListOfProducts()
        {
            // Arrange: create controller instance
            var controller = new ProductsController();

            // Act: call GetAll()
            var result = controller.GetAll();

            // Assert: result is OkObjectResult and contains products
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.NotEmpty(products);
        }

        // Test: Get product by ID
        [Fact]
        public void GetById_ExistingId_ReturnsProduct()
        {
            // Arrange: create controller and a product
            var controller = new ProductsController();
            var createResult = controller.Create(new Product { Name = "Test", Description = "Test", Price = 10m, Inventory = 5, Category = "Test" });
            var createdProduct = Assert.IsType<Product>(((CreatedAtActionResult)createResult.Result).Value);

            // Act: call GetById() for the created product
            var result = controller.GetById(createdProduct.Id);

            // Assert: result is OkObjectResult and product matches
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var productFromController = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(createdProduct.Id, productFromController.Id);
        }
    }
}
