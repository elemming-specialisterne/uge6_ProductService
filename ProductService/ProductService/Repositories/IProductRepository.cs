using ProductService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        // Returns all products in the database
        Task<IEnumerable<Product>> GetAllAsync();

        // Returns a single product by ID, or null if not found
        Task<Product?> GetByIdAsync(int id);

        // Filters products based on optional parameters
        Task<IEnumerable<Product>> FilterAsync(string? name, string? category, decimal? minPrice, decimal? maxPrice);

        // Adds a new product to the database
        Task<Product> AddAsync(Product product);

        // Updates an existing product in the database
        Task UpdateAsync(Product product);

        // Deletes a product by ID; does nothing if product not found
        Task DeleteAsync(int id);
    }
}