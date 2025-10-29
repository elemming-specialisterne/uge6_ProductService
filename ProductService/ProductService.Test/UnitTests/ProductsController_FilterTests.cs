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
    public class ProductsController_FilterTests
    {
        [Fact]
        public async Task Filter_ByName_ReturnsMatchingProducts()
        {
            // Arrange: create test products and mock repository
            var testProducts = new List<Product>
            {
                new Product { ProductID = 1, Name = "Coffee", Category = "Drinks", Price = 25m },
                new Product { ProductID = 2, Name = "Tea", Category = "Drinks", Price = 15m },
                new Product { ProductID = 3, Name = "Cocoa milk", Category = "Drinks", Price = 18m }
            };

            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.FilterAsync("co", null, null, null))
                    .ReturnsAsync(testProducts.Where(p => p.Name.ToLower().Contains("co")));

            var controller = new ProductsController(mockRepo.Object);

            // Act
            var actionResult = await controller.Filter("co", null, null, null);
            var okResult = actionResult.Result as OkObjectResult;
            var filteredProducts = okResult?.Value as IEnumerable<Product>;

            // Assert: two products should match the filter
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, p => p.Name == "Coffee");
            Assert.Contains(list, p => p.Name == "Cocoa milk");
        }

        // Additional tests follow same pattern for category and price filters...
    }
}