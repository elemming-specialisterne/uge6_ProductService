using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        // Constructor injection of repository for decoupling and testability
        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: api/products
        // Returns all products
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products);
        }

        // GET: api/products/filter
        // Filters products based on optional query parameters: name, category, minPrice, maxPrice
        [HttpGet("filter")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Product>>> Filter(
            [FromQuery] string? name,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var products = await _repository.FilterAsync(name, minPrice, maxPrice);
            return Ok(products);
        }

        // GET: api/products/{id}
        // Returns a single product by ID, or 404 if not found
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // POST: api/products
        // Creates a new product; returns 400 if the model is invalid
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> Create(Product newProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _repository.AddAsync(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = product.Productid }, product);
        }

        // PUT: api/products/{id}
        // Updates an existing product; returns 400 if ID mismatch or model invalid
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Product updatedProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updatedProduct.Productid)
                return BadRequest();

            await _repository.UpdateAsync(updatedProduct);
            return NoContent();
        }

        // DELETE: api/products/{id}
        // Deletes a product by ID
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
