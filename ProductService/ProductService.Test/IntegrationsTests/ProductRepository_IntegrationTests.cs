using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Test.IntegrationTests
{
    public class ProductRepository_IntegrationTests
    {
        // Helper to create a new in-memory DbContext for each test to ensure isolation
        private ProductDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
                .Options;

            return new ProductDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Arrange: create context and repository, add sample products
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);

            var products = new List<Product>
            {
                new Product { Name = "Coffee", Category = "Drinks", Price = 25m, Inventory = 10, Description = "Black coffee", Active = true },
                new Product { Name = "Tea", Category = "Drinks", Price = 15m, Inventory = 5, Description = "Green tea", Active = true }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            // Act: retrieve all products
            var result = await repository.GetAllAsync();

            // Assert: verify that all products are returned
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsProductSuccessfully()
        {
            // Arrange: create context and repository, prepare new product
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);

            var newProduct = new Product
            {
                Name = "Coffee",
                Category = "Drinks",
                Price = 25m,
                Inventory = 10,
                Description = "Black coffee",
                Active = true
            };

            // Act: add product and retrieve all products
            var addedProduct = await repository.AddAsync(newProduct);
            var allProducts = await repository.GetAllAsync();

            // Assert: check that the product was added correctly
            Assert.Single(allProducts);
            Assert.Equal("Coffee", addedProduct.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectProduct()
        {
            // Arrange: create context, repository and add a product
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);

            var product = new Product
            {
                Name = "Tea",
                Category = "Drinks",
                Price = 15m,
                Inventory = 5,
                Description = "Green tea",
                Active = true
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act: get product by ID
            var result = await repository.GetByIdAsync(product.ProductID);

            // Assert: verify the correct product is returned
            Assert.NotNull(result);
            Assert.Equal("Tea", result!.Name);
        }

        [Fact]
        public async Task FilterAsync_FiltersCorrectly()
        {
            // Arrange: add multiple products to test filtering
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);

            var products = new List<Product>
            {
                new Product { Name = "Coffee", Category = "Drinks", Price = 25m, Inventory = 10, Description = "Black coffee", Active = true },
                new Product { Name = "Tea", Category = "Drinks", Price = 15m, Inventory = 5, Description = "Green tea", Active = true },
                new Product { Name = "Bread", Category = "Food", Price = 10m, Inventory = 20, Description = "Whole grain", Active = true }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            // Act: filter products by category "Drinks" and minPrice 20
            var filtered = await repository.FilterAsync(null, "Drinks", 20, null);

            // Assert: only the matching product should be returned
            Assert.Single(filtered);
            Assert.Equal("Coffee", filtered.First().Name);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProductSuccessfully()
        {
            // Arrange: create context, repository, and add a product
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);

            var product = new Product
            {
                Name = "Coffee",
                Category = "Drinks",
                Price = 25m,
                Inventory = 10,
                Description = "Black coffee",
                Active = true
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Modify product
            product.Price = 30m;

            // Act: update product in database
            await repository.UpdateAsync(product);
            var updated = await repository.GetByIdAsync(product.ProductID);

            // Assert: verify that product was updated
            Assert.NotNull(updated);
            Assert.Equal(30m, updated!.Price);
        }

        [Fact]
        public async Task DeleteAsync_RemovesProductSuccessfully()
        {
            // Arrange: create context, repository, and add a product
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);

            var product = new Product
            {
                Name = "Coffee",
                Category = "Drinks",
                Price = 25m,
                Inventory = 10,
                Description = "Black coffee",
                Active = true
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act: delete the product
            await repository.DeleteAsync(product.ProductID);
            var allProducts = await repository.GetAllAsync();

            // Assert: product list should be empty
            Assert.Empty(allProducts);
        }
    }
}
