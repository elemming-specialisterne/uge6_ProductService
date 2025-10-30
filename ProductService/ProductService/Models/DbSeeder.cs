using ProductService.Models;

namespace ProductService.Data
{
    public static class DbSeeder
    {
        public static void Seed(ProductContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Skip seeding if data already exists
            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product { Name = "Widget", Description = "Nice widget", Price = 19.99m },
                new Product { Name = "Cable", Description = "Cat6 ethernet cable 2m", Price = 5.00m },
                new Product { Name = "Mouse", Description = "Ergonomic optical mouse", Price = 14.50m },
                new Product { Name = "Keyboard", Description = "Compact mechanical keyboard", Price = 59.00m },
                new Product { Name = "Monitor", Description = "24-inch IPS monitor", Price = 129.99m },
                new Product { Name = "USB Hub", Description = "4-port USB 3.0 hub", Price = 17.95m },
                new Product { Name = "SSD", Description = "512GB SATA SSD", Price = 49.99m },
                new Product { Name = "HDMI Cable", Description = "High-speed HDMI cable 1.5m", Price = 6.50m },
                new Product { Name = "Webcam", Description = "1080p USB webcam", Price = 39.00m },
                new Product { Name = "Headset", Description = "Stereo headset with mic", Price = 29.90m }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
