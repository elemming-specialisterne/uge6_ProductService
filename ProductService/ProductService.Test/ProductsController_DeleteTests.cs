using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Models;
using Xunit;

namespace ProductService.Test
{
    public class ProductsController_DeleteTests
    {
        // Test: DELETE existing product
        [Fact]
        public void Delete_ExistingProduct_RemovesProduct()
        {
            // Arrange: create controller and a product
            var controller = new ProductsController();
            var createResult = controller.Create(new Product { Name = "SletMig", Description = "Test", Price = 15m, Inventory = 10, Category = "Test", Supplier = "Unit", Barcode = "DEL1" });
            var createdProduct = Assert.IsType<Product>(((CreatedAtActionResult)createResult.Result).Value);

            // Act: call Delete()
            var deleteResult = controller.Delete(createdProduct.Id);

            // Assert: returns NoContentResult and product is removed
            Assert.IsType<NoContentResult>(deleteResult);
            var getResult = controller.GetById(createdProduct.Id);
            Assert.IsType<NotFoundResult>(getResult.Result);
        }

        // Test: DELETE non-existing product
        [Fact]
        public void Delete_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange: create controller
            var controller = new ProductsController();

            // Act: attempt to delete non-existing product
            var result = controller.Delete(999);

            // Assert: returns NotFoundResult
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
