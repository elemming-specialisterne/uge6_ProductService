using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Models;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace ProductService.Test
{
    public class ProductsController_FilterTests
    {
        // Test: Filter by category and price range
        [Fact]
        public void Filter_ByCategoryAndPrice_ReturnsCorrectProducts()
        {
            // Arrange: create test products and controller instance
            var testProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Kaffe", Category = "Drikkevarer", Price = 25m },
                new Product { Id = 2, Name = "Te", Category = "Drikkevarer", Price = 15m },
                new Product { Id = 3, Name = "Brød", Category = "Mad", Price = 10m }
            };
            var controller = new ProductsController(testProducts);

            // Act: filter products by category "Drikkevarer" and price between 20 and 30
            var result = controller.Filter("Drikkevarer", 20, 30).Result as OkObjectResult;
            var filteredProducts = result?.Value as IEnumerable<Product>;

            // Assert: only one product ("Kaffe") matches
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Single(list);
            Assert.Equal("Kaffe", list[0].Name);
        }

        // Test: Filter by category only
        [Fact]
        public void Filter_ByCategoryOnly_ReturnsAllProductsInCategory()
        {
            // Arrange: create test products and controller instance
            var testProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Kaffe", Category = "Drikkevarer", Price = 25m },
                new Product { Id = 2, Name = "Te", Category = "Drikkevarer", Price = 15m },
                new Product { Id = 3, Name = "Brød", Category = "Mad", Price = 10m }
            };
            var controller = new ProductsController(testProducts);

            // Act: filter products by category "Drikkevarer"
            var result = controller.Filter("Drikkevarer", null, null).Result as OkObjectResult;
            var filteredProducts = result?.Value as IEnumerable<Product>;

            // Assert: two products ("Kaffe" and "Te") match
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(2, list.Count);
        }

        // Test: Filter by price range only
        [Fact]
        public void Filter_ByPriceOnly_ReturnsAllProductsInPriceRange()
        {
            // Arrange: create test products and controller instance
            var testProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Kaffe", Category = "Drikkevarer", Price = 25m },
                new Product { Id = 2, Name = "Te", Category = "Drikkevarer", Price = 15m },
                new Product { Id = 3, Name = "Brød", Category = "Mad", Price = 10m }
            };
            var controller = new ProductsController(testProducts);

            // Act: filter products with price between 10 and 20
            var result = controller.Filter(null, 10, 20).Result as OkObjectResult;
            var filteredProducts = result?.Value as IEnumerable<Product>;

            // Assert: two products ("Te" and "Brød") match
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(2, list.Count);
        }

        // Test: No filter parameters
        [Fact]
        public void Filter_NoParameters_ReturnsAllProducts()
        {
            // Arrange: create test products and controller instance
            var testProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Kaffe", Category = "Drikkevarer", Price = 25m },
                new Product { Id = 2, Name = "Te", Category = "Drikkevarer", Price = 15m },
                new Product { Id = 3, Name = "Brød", Category = "Mad", Price = 10m }
            };
            var controller = new ProductsController(testProducts);

            // Act: call filter without parameters
            var result = controller.Filter(null, null, null).Result as OkObjectResult;
            var filteredProducts = result?.Value as IEnumerable<Product>;

            // Assert: all products are returned
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(3, list.Count);
        }
    }
}
