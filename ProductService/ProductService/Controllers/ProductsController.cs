using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // In-memory liste som "lager"
        private List<Product> products = new()
        {
            new Product { Id = 1, Name = "Kaffe", Description = "Sort kaffe", Price = 25.50m, Inventory = 100, Category = "Drikkevarer", Supplier = "KaffeFirma", Barcode = "1234567890" },
            new Product { Id = 2, Name = "Brød", Description = "Franskbrød", Price = 15.00m, Inventory = 50, Category = "Mad", Supplier = "Bageren", Barcode = "0987654321" },
            new Product { Id = 3, Name = "Te", Description = "Grøn te", Price = 20.00m, Inventory = 80, Category = "Drikkevarer", Supplier = "TeHuset", Barcode = "1122334455" },
            new Product { Id = 4, Name = "Mælk", Description = "Letmælk 1L", Price = 10.50m, Inventory = 200, Category = "Mad", Supplier = "MejeriNord", Barcode = "2233445566" },
            new Product { Id = 5, Name = "Smør", Description = "Økologisk smør 250g", Price = 18.75m, Inventory = 75, Category = "Mad", Supplier = "Bageren", Barcode = "3344556677" },
            new Product { Id = 6, Name = "Æbler", Description = "Røde æbler, 1kg", Price = 22.00m, Inventory = 120, Category = "Frugt", Supplier = "FrugtTorvet", Barcode = "4455667788" },
            new Product { Id = 7, Name = "Sodavand", Description = "Cola 0.5L", Price = 12.50m, Inventory = 150, Category = "Drikkevarer", Supplier = "SodavandsFabrikken", Barcode = "5566778899" },
            new Product { Id = 8, Name = "Chokolade", Description = "Mørk chokolade 100g", Price = 14.00m, Inventory = 90, Category = "Slik", Supplier = "ChocoLand", Barcode = "6677889900" },
            new Product { Id = 9, Name = "Pasta", Description = "Spaghetti 500g", Price = 13.50m, Inventory = 110, Category = "Mad", Supplier = "ItalienskImport", Barcode = "7788990011" },
            new Product { Id = 10, Name = "Ost", Description = "Cheddar 200g", Price = 24.00m, Inventory = 60, Category = "Mad", Supplier = "MejeriNord", Barcode = "8899001122" }
        };

        // GET: api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public ActionResult<Product> Create(Product newProduct)
        {
            newProduct.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            products.Add(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Inventory = updatedProduct.Inventory;
            product.Category = updatedProduct.Category;
            product.Supplier = updatedProduct.Supplier;
            product.Barcode = updatedProduct.Barcode;
            product.Active = updatedProduct.Active;

            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            products.Remove(product);
            return NoContent();
        }
    }
}
