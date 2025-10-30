using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        // Constructor: injects the DbContext
        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // Returns all products as a list asynchronously
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            // Finds a product by primary key asynchronously
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> FilterAsync(string? name, string? category, decimal? minPrice, decimal? maxPrice)
        {
            // Start building queryable collection
            var query = _context.Products.AsQueryable();

            // Apply name filter if provided
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name));

            // Apply minimum price filter if provided
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            // Apply maximum price filter if provided
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            // Execute query and return filtered list
            return await query.ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            // Add new product to the DbSet and save changes
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            // Update product in the DbSet and save changes
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Find product by ID
            var product = await _context.Products.FindAsync(id);
            if (product == null) return; // Do nothing if not found

            // Remove product and save changes
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}