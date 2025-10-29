using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using System.Collections.Generic;

namespace ProductService.Data
{
    public class ProductDbContext : DbContext
    {
        // Constructor: pass DbContextOptions to the base DbContext
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        // Represents the Products table in the database
        public DbSet<Product> Products { get; set; }
    }
}