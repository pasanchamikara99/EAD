using E_commerce_system.Data;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace E_commerce_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : Controller
    {
        private readonly IMongoCollection<Product>? _products;

        private readonly IMongoCollection<Category> _categories;

        public InventoryController(MongoDbService mongoDbService)
        {
            _categories = mongoDbService.Database?.GetCollection<Category>("category");
            _products = mongoDbService.Database?.GetCollection<Product>("product");
        }

        [HttpPost("addCategory")]
        public async Task<Category> AddCategoryAsync(Category category)
        {
            await _categories.InsertOneAsync(category);
            return category;
        }

        [HttpGet("getCategories")]
        public async Task<List<Category>> GetCategoriesAsync()
        {
            // Fetch all categories from the collection
            var categories = await _categories.Find(_ => true).ToListAsync();
            return categories;
        }


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

        public class CategoryProductCount
        {
            public string CategoryName { get; set; }
            public int ProductCount { get; set; }
        }


    }
}
