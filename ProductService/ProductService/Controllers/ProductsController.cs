using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly List<Product> products;

        // Constructor that allows injecting test data
        public ProductsController(List<Product>? initialProducts = null)
        {
            products = initialProducts ?? new List<Product>
            {
                new Product { ProductID = 1, Name = "Coffee", Description = "Black coffee", Price = 25.50m, Inventory = 100, Category = "Drinks" },
                new Product { ProductID = 2, Name = "Bread", Description = "White bread", Price = 15.00m, Inventory = 50, Category = "Food" },
                new Product { ProductID = 3, Name = "Tea", Description = "Green tea", Price = 20.00m, Inventory = 80, Category = "Drinks" },
                new Product { ProductID = 4, Name = "Milk", Description = "Low fat milk 1L", Price = 10.50m, Inventory = 200, Category = "Food" },
                new Product { ProductID = 5, Name = "Butter", Description = "Organic butter 250g", Price = 18.75m, Inventory = 75, Category = "Food" },
                new Product { ProductID = 6, Name = "Apples", Description = "Red apples, 1kg", Price = 22.00m, Inventory = 120, Category = "Fruit" },
                new Product { ProductID = 7, Name = "Soda", Description = "Coca cola 0.5L", Price = 12.50m, Inventory = 150, Category = "Drinks" },
                new Product { ProductID = 8, Name = "Chocolate", Description = "Dark chocolate 100g", Price = 14.00m, Inventory = 90, Category = "Candy" },
                new Product { ProductID = 9, Name = "Pasta", Description = "Spaghetti 500g", Price = 13.50m, Inventory = 110, Category = "Food" },
                new Product { ProductID = 10, Name = "Cheese", Description = "Cheddar 200g", Price = 24.00m, Inventory = 60, Category = "Food" }
            };
        }

        // GET: api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(products);
        }

        // GET: api/products/filter?category=Food&minPrice=10&maxPrice=25
        [HttpGet("filter")]
        public ActionResult<IEnumerable<Product>> Filter([FromQuery] string? name, [FromQuery] string? category, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var filtered = products.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(name)) 
                filtered = filtered.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(category))
                filtered = filtered.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (minPrice.HasValue)
                filtered = filtered.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                filtered = filtered.Where(p => p.Price <= maxPrice.Value);

            return Ok(filtered);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = products.FirstOrDefault(p => p.ProductID == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public ActionResult<Product> Create(Product newProduct)
        {
            newProduct.ProductID = products.Any() ? products.Max(p => p.ProductID) + 1 : 1;
            products.Add(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.ProductID }, newProduct);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.ProductID == id);
            if (product == null) return NotFound();

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Inventory = updatedProduct.Inventory;
            product.Category = updatedProduct.Category;
            product.Active = updatedProduct.Active;

            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.ProductID == id);
            if (product == null) return NotFound();

            products.Remove(product);
            return NoContent();
        }

    }
}
