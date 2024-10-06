using E_commerce_system.Data;
using E_commerce_system.Data.Entities;
using E_commerce_system.Data.Services;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace E_commerce_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IMongoCollection<Product>? _products;

        private readonly IVendorService _vendorService;

        public ProductController(MongoDbService mongoDbService)
        {

            _products = mongoDbService.Database?.GetCollection<Product>("product");
        }

        [HttpGet("getAllProducts")]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _products.Find(FilterDefinition<Product>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product?>> GetById(string id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var product = await _products.Find(filter).FirstOrDefaultAsync();
            return product is not null ? Ok(product) : NotFound();
        }

        [HttpPost("addProduct")]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            product.Status = "Pending";
            await _products.InsertOneAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpGet("productsByCategory/{category}")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(string category)
        {
            
            var filter = Builders<Product>.Filter.Eq(p => p.ProductCategory, category);

           
            var products = await _products.Find(filter).ToListAsync();

           
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "No products found in this category." });
            }

        
            return Ok(products);
        }

        [HttpGet("productsByVendor/{vendorId}")]
        public async Task<ActionResult<List<Product>>> GetProductsByVendor(string vendorId)
        {
           
            var filter = Builders<Product>.Filter.Eq(p => p.VendorId, vendorId);

           
            var products = await _products.Find(filter).ToListAsync();

           
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "No products found for this vendor." });
            }

           
            return Ok(products);
        }

        [HttpDelete("deleteProduct/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            // Check if the product exists
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var product = await _products.Find(filter).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            // Delete the product
            await _products.DeleteOneAsync(filter);
            return NoContent(); // Respond with 204 No Content
        }


        [HttpPut("updateProduct/{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] Product updatedProduct)
        {
            // Check if the product exists
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var existingProduct = await _products.Find(filter).FirstOrDefaultAsync();

            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            var update = Builders<Product>.Update
                .Set(x => x.ProductName, updatedProduct.ProductName)
                .Set(x => x.ProductDescription, updatedProduct.ProductDescription)
                .Set(x => x.ProductCategory, updatedProduct.ProductCategory)
                .Set(x => x.ProductQuantity, updatedProduct.ProductQuantity)
                .Set(x => x.UnitPrice, updatedProduct.UnitPrice)
                .Set(x => x.VendorId, updatedProduct.VendorId)
                .Set(x => x.Status, updatedProduct.Status)
                .Set(x => x.Image, updatedProduct.Image);

            // Perform the update
            await _products.UpdateOneAsync(filter, update);

            return Ok(new { message = "Product updated successfully." });
        }


    }
}
