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
        // Test: Filter by name (partial match)
        [Fact]
        public void Filter_ByName_ReturnsMatchingProducts()
        {
            // Arrange: create test products and controller instance
            var testProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Coffee", Category = "Drinks", Price = 25m },
                new Product { Id = 2, Name = "Tea", Category = "Drinks", Price = 15m },
                new Product { Id = 3, Name = "Cocoa milk", Category = "Drinks", Price = 18m }
            };
            var controller = new ProductsController(testProducts);

            // Act: filter products by name containing "co"
            var result = controller.Filter("co", null, null, null).Result as OkObjectResult;
            var filteredProducts = result?.Value as IEnumerable<Product>;

            // Assert: both "Coffee" and "Cocoa milk" should match
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, p => p.Name == "Coffee");
            Assert.Contains(list, p => p.Name == "Cocoa milk");
        }

        // Test: Filter by category and price range
        [Fact]
        public void Filter_ByCategoryAndPrice_ReturnsCorrectProducts()
        {
            // Arrange: create test products and controller instance
            var testProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Coffee", Category = "Drinks", Price = 25m },
                new Product { Id = 2, Name = "Tea", Category = "Drinks", Price = 15m },
                new Product { Id = 3, Name = "Bread", Category = "Food", Price = 10m }
            };
            var controller = new ProductsController(testProducts);

            // Act: filter products by category "Drinks" and price between 20 and 30
            var result = controller.Filter("Coffee", "Drinks", 20, 30).Result as OkObjectResult;
            var filteredProducts = result?.Value as IEnumerable<Product>;

            // Assert: only one product ("Coffee") matches
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Single(list);
            Assert.Equal("Coffee", list[0].Name);
        }

        // Test: Filter by category only
        [Fact]
        public void Filter_ByCategoryOnly_ReturnsAllProductsInCategory()
        {
            // Arrange: create test products and controller instance
            var testProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Coffee", Category = "Drinks", Price = 25m },
                new Product { Id = 2, Name = "Tea", Category = "Drinks", Price = 15m },
                new Product { Id = 3, Name = "Bread", Category = "Food", Price = 10m }
            };
            var controller = new ProductsController(testProducts);

            // Act: filter products by category "Drinks"
            var result = controller.Filter(null, "Drinks", null, null).Result as OkObjectResult;
            var filteredProducts = result?.Value as IEnumerable<Product>;

            // Assert: two products ("Coffee" and "Tea") match
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
                new Product { Id = 1, Name = "Coffee", Category = "Drinks", Price = 25m },
                new Product { Id = 2, Name = "Tea", Category = "Drinks", Price = 15m },
                new Product { Id = 3, Name = "Bread", Category = "Food", Price = 10m }
            };
            var controller = new ProductsController(testProducts);

            // Act: filter products with price between 10 and 20
            var result = controller.Filter(null, null, 10, 20).Result as OkObjectResult;
            var filteredProducts = result?.Value as IEnumerable<Product>;

            // Assert: two products ("Tea" and "Bread") match
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
                new Product { Id = 1, Name = "Coffee", Category = "Drinks", Price = 25m },
                new Product { Id = 2, Name = "Tea", Category = "Drinks", Price = 15m },
                new Product { Id = 3, Name = "Bread", Category = "Food", Price = 10m }
            };
            var controller = new ProductsController(testProducts);

            // Act: call filter without parameters
            var result = controller.Filter(null, null, null, null).Result as OkObjectResult;
            var filteredProducts = result?.Value as IEnumerable<Product>;

            // Assert: all products are returned
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(3, list.Count);
        }
    }
}
