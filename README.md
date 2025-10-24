# ProductService – Inventory Management API

## 1. Project Overview

ProductService is a simple ASP.NET Core Web API for managing products in an inventory system. The API allows for CRUD operations on products and provides an interactive interface via Swagger.

---

## 2. API Endpoints

| HTTP Method | Endpoint | Description |
|-------------|----------|-------------|
| GET         | /api/products | Retrieve all products |
| GET         | /api/products/{id} | Retrieve a product by ID |
| POST        | /api/products | Create a new product |
| PUT         | /api/products/{id} | Update an existing product |
| DELETE      | /api/products/{id} | Delete a product by ID |

---

## 3. Test Environment

- **IDE:** Visual Studio 2022  
- **.NET Version:** 8.0 (or latest installed)  
- **Packages:**  
  - Swashbuckle.AspNetCore (Swagger/OpenAPI support)
  - Xunit (Unit testing framework)

---

## 4. In-Memory Data

The API uses an **in-memory list** to simulate a database. Sample products included:

| Id | Name       | Category     | Inventory | Price  | Supplier          |
|----|------------|-------------|-----------|--------|------------------|
| 1  | Kaffe      | Beverages    | 100       | 25.50  | KaffeFirma       |
| 2  | Brød       | Food         | 50        | 15.00  | Bageren          |
| 3  | Te         | Beverages    | 80        | 20.00  | TeHuset          |
| 4  | Mælk       | Food         | 200       | 10.50  | MejeriNord       |
| 5  | Smør       | Food         | 75        | 18.75  | Bageren          |
| ...| ...        | ...          | ...       | ...    | ...              |

> 💡 The full list contains 10 sample products for testing purposes.

---

## 5. Swagger UI

- Swagger is available for testing all endpoints interactively.  
- To use Swagger:  
  1. Start the project (F5 or Ctrl + F5).  
  2. Open the browser at: `https://localhost:7125/` (or HTTP port if using non-HTTPS).  
  3. You can test GET, POST, PUT, DELETE requests directly from the UI.

---

## 6. Code Structure

- **Models:**  
  - `Product` class contains the product properties:
    - `Id`, `Name`, `Description`, `Price`, `Inventory`, `Category`, `Supplier`, `Barcode`, `CreatedDate`, `Active`
- **Controllers:**  
  - `ProductsController` handles all CRUD operations for products.
- **Program.cs:**  
  - Configures services, Swagger, and routes.

---

## 7. Unit Tests

The project includes separate test classes for each CRUD operation. Each test follows the **Arrange / Act / Assert** pattern.

| Test Class | Test Method | Purpose | Arrange | Act | Assert |
|------------|------------|---------|--------|-----|-------|
| `ProductsController_GetTests` | `GetAll_ReturnsListOfProducts` | Ensure `GetAll` returns a non-empty list | Create controller | Call `GetAll()` | Result is `OkObjectResult` and list is not empty |
| `ProductsController_GetTests` | `GetById_ExistingId_ReturnsProduct` | Ensure `GetById` returns the correct product | Create controller and product | Call `GetById(createdProduct.Id)` | Result is `OkObjectResult` and product matches |
| `ProductsController_PostTests` | `Create_ValidProduct_ReturnsCreatedProduct` | Ensure `Create` adds a product | Create controller and new product | Call `Create(newProduct)` | Result is `CreatedAtActionResult` and product has Id assigned |
| `ProductsController_PutTests` | `Update_ExistingProduct_UpdatesValuesCorrectly` | Ensure updating an existing product changes its properties | Create controller and add product | Call `Update(createdProduct.Id, updatedProduct)` | Result is `NoContentResult` and product values updated |
| `ProductsController_PutTests` | `Update_NonExistingProduct_ReturnsNotFound` | Ensure updating a non-existing product returns NotFound | Create controller and fake product | Call `Update(999, nonExistingProduct)` | Result is `NotFoundResult` |
| `ProductsController_DeleteTests` | `Delete_ExistingProduct_RemovesProduct` | Ensure deleting an existing product removes it | Create controller and product | Call `Delete(createdProduct.Id)` | Result is `NoContentResult` and product cannot be retrieved |
| `ProductsController_DeleteTests` | `Delete_NonExistingProduct_ReturnsNotFound` | Ensure deleting a non-existing product returns NotFound | Create controller | Call `Delete(999)` | Result is `NotFoundResult` |

> 💡 All tests are independent; each test creates its own product instances to avoid side effects.

---
