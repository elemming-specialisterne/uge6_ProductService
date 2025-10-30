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
            // Arrange: create test products and mock repository for name filter
            var testProducts = new List<Product>
            {
                new Product { Productid = 1, Name = "Coffee", Price = 25m },
                new Product { Productid = 2, Name = "Tea", Price = 15m },
                new Product { Productid = 3, Name = "Cocoa milk", Price = 18m }
            };

            // Mock repository to return products matching the name filter
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.FilterAsync("co", null, null))
                    .ReturnsAsync(testProducts.Where(p => p.Name.ToLower().Contains("co")));

            var controller = new ProductsController(mockRepo.Object);

            // Act: call the controller filter for name
            var actionResult = await controller.Filter("co", null, null);
            var okResult = actionResult.Result as OkObjectResult;
            var filteredProducts = okResult?.Value as IEnumerable<Product>;

            // Assert: two products should match the filter by name
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, p => p.Name == "Coffee");
            Assert.Contains(list, p => p.Name == "Cocoa milk");
        }

        [Fact]
        public async Task Filter_ByMinPrice_ReturnsProductsAboveMinPrice()
        {
            // Arrange: create test products with minimum price filter 
            var testProducts = new List<Product>
            {
                new Product { Productid = 1, Name = "Coffee", Price = 25m },
                new Product { Productid = 2, Name = "Tea", Price = 15m },
                new Product { Productid = 3, Name = "Cocoa milk", Price = 18m }
            };

            // Mock repository to return products above the specified minimum price
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.FilterAsync(null, 20, null))
                    .ReturnsAsync(testProducts.Where(p => p.Price >= 20));

            var controller = new ProductsController(mockRepo.Object);

            // Act: call the controller filter for minimum price
            var actionResult = await controller.Filter(null, 20, null);
            var okResult = actionResult.Result as OkObjectResult;
            var filteredProducts = okResult?.Value as IEnumerable<Product>;

            // Assert: only one product should match the filter by minimum price
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Single(list);
            Assert.Equal("Coffee", list[0].Name);
        }

        [Fact]
        public async Task Filter_ByMaxPrice_ReturnsProductsBelowMaxPrice()
        {
            // Arrange: create test products with maximum price filter
            var testProducts = new List<Product>
            {
                new Product { Productid = 1, Name = "Coffee", Price = 25m },
                new Product { Productid = 2, Name = "Tea", Price = 15m },
                new Product { Productid = 3, Name = "Cocoa milk", Price = 18m }
            };

            // Mock repository to return products below the specified maximum price
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.FilterAsync(null, null, 18))
                    .ReturnsAsync(testProducts.Where(p => p.Price <= 18));

            var controller = new ProductsController(mockRepo.Object);

            // Act: call the controller filter for maximum price
            var actionResult = await controller.Filter(null, null, 18);
            var okResult = actionResult.Result as OkObjectResult;
            var filteredProducts = okResult?.Value as IEnumerable<Product>;

            // Assert: two products should match the filter by maximum price
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, p => p.Name == "Tea");
            Assert.Contains(list, p => p.Name == "Cocoa milk");
        }

        [Fact]
        public async Task Filter_ByMinAndMaxPrice_ReturnsProductsWithinRange()
        {
            // Arrange: create test products with both minimum and maximum price filters
            var testProducts = new List<Product>
            {
                new Product { Productid = 1, Name = "Coffee", Price = 25m },
                new Product { Productid = 2, Name = "Tea", Price = 15m },
                new Product { Productid = 3, Name = "Cocoa milk", Price = 18m }
            };

            // Mock repository to return products within the specified price range
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.FilterAsync(null, 15, 20))
                    .ReturnsAsync(testProducts.Where(p => p.Price >= 15 && p.Price <= 20));

            var controller = new ProductsController(mockRepo.Object);

            // Act: call the controller filter for price range
            var actionResult = await controller.Filter(null, 15, 20);
            var okResult = actionResult.Result as OkObjectResult;
            var filteredProducts = okResult?.Value as IEnumerable<Product>;

            // Assert: two products should match the filter within the price range
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, p => p.Name == "Tea");
            Assert.Contains(list, p => p.Name == "Cocoa milk");
        }

        [Fact]
        public async Task Filter_ByNameAndPrice_ReturnsMatchingProducts()
        {
            // Arrange: create test products
            var testProducts = new List<Product>
            {
                new Product { Productid = 1, Name = "Coffee", Price = 25m },
                new Product { Productid = 2, Name = "Tea", Price = 15m },
                new Product { Productid = 3, Name = "Cocoa milk", Price = 18m },
                new Product { Productid = 4, Name = "Coffee Latte", Price = 20m }
            };

            // Mock repository to return products that match both name and price filters
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.FilterAsync("coffee", 20, 25))
                    .ReturnsAsync(testProducts.Where(p =>
                        p.Name.ToLower().Contains("coffee") &&
                        p.Price >= 20 && p.Price <= 25));

            var controller = new ProductsController(mockRepo.Object);

            // Act: call the controller filter
            var actionResult = await controller.Filter("coffee", 20, 25);
            var okResult = actionResult.Result as OkObjectResult;
            var filteredProducts = okResult?.Value as IEnumerable<Product>;

            // Assert: only products matching both name and price should be returned
            Assert.NotNull(filteredProducts);
            var list = filteredProducts!.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, p => p.Name == "Coffee");
            Assert.Contains(list, p => p.Name == "Coffee Latte");
        }
    }
}