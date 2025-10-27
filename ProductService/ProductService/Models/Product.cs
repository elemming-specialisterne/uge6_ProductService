namespace ProductService.Models
{
    public class Product
    {
        public int Id { get; set; } // Unique product ID
        public string Name { get; set; } // Product name
        public string Description { get; set; } // Short description
        public decimal Price { get; set; } // Sale price
        public int Inventory { get; set; } // Quantity in stock
        public string Category { get; set; } // E.g., Beverages, Food
        public bool Active { get; set; } = true; // Whether the product is active/available
    }
}