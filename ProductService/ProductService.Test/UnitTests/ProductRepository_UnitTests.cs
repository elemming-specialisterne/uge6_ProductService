using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Test.UnitTests
{
    public class ProductRepository_UnitTests
    {
        // Creates a new in-memory database for each test
        private ProductRepository CreateRepository(string dbName)
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ProductContext(options);
            return new ProductRepository(context);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Arrange – add multiple products to the in-memory database
            var repository = CreateRepository(nameof(GetAllAsync_ReturnsAllProducts));

            await repository.AddAsync(new Product { Name = "Coffee", Description = "Hot drink", Price = 25m });
            await repository.AddAsync(new Product { Name = "Tea", Description = "Hot drink", Price = 15m });
            await repository.AddAsync(new Product { Name = "Bread", Description = "Bakery", Price = 10m });

            // Act – retrieve all products
            var result = await repository.GetAllAsync();

            // Assert – verify that 3 products are returned
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectProduct()
        {
            // Arrange – add one product
            var repository = CreateRepository(nameof(GetByIdAsync_ReturnsCorrectProduct));
            var product = await repository.AddAsync(new Product { Name = "Coffee", Description = "Hot drink", Price = 25m });

            // Act – get it by ID
            var found = await repository.GetByIdAsync(product.Productid);

            // Assert – verify that the product is found and matches
            Assert.NotNull(found);
            Assert.Equal("Coffee", found!.Name);
        }

        [Fact]
        public async Task AddAsync_AddsProductSuccessfully()
        {
            // Arrange – create a new product
            var repository = CreateRepository(nameof(AddAsync_AddsProductSuccessfully));
            var newProduct = new Product
            {
                Name = "Coffee",
                Description = "Black coffee",
                Price = 25m
            };

            // Act – add product to database
            await repository.AddAsync(newProduct);
            var all = await repository.GetAllAsync();

            // Assert – ensure the product was saved correctly
            Assert.Single(all);
            Assert.Equal("Coffee", all.First().Name);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProductSuccessfully()
        {
            // Arrange – create and add a product
            var repository = CreateRepository(nameof(UpdateAsync_UpdatesProductSuccessfully));
            var product = await repository.AddAsync(new Product
            {
                Name = "Coffee",
                Description = "Hot drink",
                Price = 25m
            });

            // Act – change the price and update
            product.Price = 30m;
            await repository.UpdateAsync(product);
            var updated = await repository.GetByIdAsync(product.Productid);

            // Assert – confirm the price is updated
            Assert.Equal(30m, updated!.Price);
        }

        [Fact]
        public async Task DeleteAsync_RemovesProduct()
        {
            // Arrange – add a product to the database
            var repository = CreateRepository(nameof(DeleteAsync_RemovesProduct));
            var product = await repository.AddAsync(new Product
            {
                Name = "Coffee",
                Description = "Hot drink",
                Price = 25m
            });

            // Act – delete the product by ID
            await repository.DeleteAsync(product.Productid);
            var all = await repository.GetAllAsync();

            // Assert – ensure the product was deleted
            Assert.Empty(all);
        }

        [Fact]
        public async Task FilterAsync_FiltersCorrectly()
        {
            // Arrange – add several products with different categories and prices
            var repository = CreateRepository(nameof(FilterAsync_FiltersCorrectly));

            var products = new List<Product>
            {
                new Product { Name = "Coffee", Description = "Black coffee", Price = 25m },
                new Product { Name = "Tea", Description = "Green tea", Price = 15m }
            };

            foreach (var p in products)
                await repository.AddAsync(p);

            // Act – filter only drinks between 10 and 25 DKK
            var result = await repository.FilterAsync(null, 10, 25);

            // Assert – should return exactly 2 drinks
            Assert.Equal(2, result.Count());
        }
    }
}