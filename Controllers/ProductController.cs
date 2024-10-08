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

        // Method: Get
        // Purpose: Retrieves all products from the database.
        // Returns: A list of all products as an IEnumerable<Product>.
        [HttpGet("getAllProducts")]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _products.Find(FilterDefinition<Product>.Empty).ToListAsync();
        }

        // Method: GetById
        // Purpose: Retrieves a specific product by its ID.
        // Parameters: id - The ID of the product to retrieve.
        // Returns: The product if found; otherwise, a 404 Not Found response.
        [HttpGet("{id}")]
        public async Task<ActionResult<Product?>> GetById(string id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var product = await _products.Find(filter).FirstOrDefaultAsync();
            return product is not null ? Ok(product) : NotFound();
        }

        // Method: Post
        // Purpose: Adds a new product to the database.
        // Parameters: product - The product object to be added.
        // Returns: A 201 Created response with the added product.
        [HttpPost("addProduct")]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            product.Status = "Pending";
            await _products.InsertOneAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // Method: GetProductsByCategory
        // Purpose: Retrieves products that belong to a specific category.
        // Parameters: category - The category to filter products by.
        // Returns: A list of products in the specified category; otherwise, a 404 Not Found response.
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

        // Method: GetProductsByVendor
        // Purpose: Retrieves products associated with a specific vendor.
        // Parameters: vendorId - The ID of the vendor to filter products by.
        // Returns: A list of products from the specified vendor; otherwise, a 404 Not Found response.
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

        // Method: Delete
        // Purpose: Deletes a product by its ID.
        // Parameters: id - The ID of the product to delete.
        // Returns: A 204 No Content response if successful; otherwise, a 404 Not Found response.
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


        // Method: Update
        // Purpose: Updates an existing product's details.
        // Parameters: id - The ID of the product to update; updatedProduct - The updated product object.
        // Returns: A message indicating successful update; otherwise, a 404 Not Found response.
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


        // Method: ApproveProduct
        // Purpose: Approves a product by updating its status to "Approve".
        // Parameters: id - The ID of the product to approve.
        // Returns: A message indicating successful approval; otherwise, a 404 Not Found response.
        [HttpPut("approveProduct/{id}")]
        public async Task<ActionResult> ApproveProduct(string id)
        {
            // Check if the product exists
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var existingProduct = await _products.Find(filter).FirstOrDefaultAsync();

            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            // Create an update definition to set the Status to "Approve"
            var update = Builders<Product>.Update.Set(x => x.Status, "Approve");

            // Perform the update
            await _products.UpdateOneAsync(filter, update);

            return Ok(new { message = "Product status updated to 'Approve' successfully." });
        }


        // Method: RejectProduct
        // Purpose: Rejects a product by updating its status to "Reject".
        // Parameters: id - The ID of the product to reject.
        // Returns: A message indicating successful rejection; otherwise, a 404 Not Found response.
        [HttpPut("rejectProduct/{id}")]
        public async Task<ActionResult> RejectProduct(string id)
        {
            // Check if the product exists
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var existingProduct = await _products.Find(filter).FirstOrDefaultAsync();

            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            // Create an update definition to set the Status to "Approve"
            var update = Builders<Product>.Update.Set(x => x.Status, "Reject");

            // Perform the update
            await _products.UpdateOneAsync(filter, update);

            return Ok(new { message = "Product status updated to 'Reject' successfully." });
        }




    }
}
