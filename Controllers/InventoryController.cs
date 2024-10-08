using E_commerce_system.Data;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/*
 * File: InventoryController.cs
 * Author: Pasan Chamikara
 * Purpose: Manages inventory operations, including adding categories, retrieving categories, and counting products by category.
 */
namespace E_commerce_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : Controller
    {
        private readonly IMongoCollection<Product>? _products;

        private readonly IMongoCollection<Category> _categories;

        // Constructor: Initializes the controller with MongoDB collections for categories and products.
        public InventoryController(MongoDbService mongoDbService)
        {
            _categories = mongoDbService.Database?.GetCollection<Category>("category");
            _products = mongoDbService.Database?.GetCollection<Product>("product");
        }

        // Method: AddCategoryAsync
        // Purpose: Adds a new category to the database.
        // Parameter: category - The category to be added.
        // Returns: The added category.
        [HttpPost("addCategory")]
        public async Task<Category> AddCategoryAsync(Category category)
        {
            await _categories.InsertOneAsync(category);
            return category;
        }

        // Method: GetCategoriesAsync
        // Purpose: Retrieves all categories from the database.
        // Returns: A list of categories.
        [HttpGet("getCategories")]
        public async Task<List<Category>> GetCategoriesAsync()
        {
            // Fetch all categories from the collection
            var categories = await _categories.Find(_ => true).ToListAsync();
            return categories;
        }


        // Method: GetProductCountByCategory
        // Purpose: Retrieves the count of products grouped by category.
        // Returns: A list of CategoryProductCount objects containing category names and product counts.
        [HttpGet("countByCategory")]
        public async Task<ActionResult<List<CategoryProductCount>>> GetProductCountByCategory()
        {
            // Retrieve all products
            var products = await _products.Find(_ => true).ToListAsync();

            // Group by category and count
            var categoryCounts = products
                .GroupBy(p => p.ProductCategory)
                .Select(g => new CategoryProductCount
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count()
                })
                .ToList();

            return Ok(categoryCounts);
        }

        // Nested class: CategoryProductCount
        // Purpose: Represents the count of products in a category.
        public class CategoryProductCount
        {
            public string CategoryName { get; set; }
            public int ProductCount { get; set; }
        }


    }
}
