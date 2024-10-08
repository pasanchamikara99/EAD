using E_commerce_system.Data;
using E_commerce_system.Data.Entities;
using E_commerce_system.DTO;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/*
 * File: DashboardController.cs
 * Author: Pasan Chamikara
 * Purpose: Manages dashboard operations for the admin, providing counts of orders, customers, products, and vendors.
 */
namespace E_commerce_system.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IMongoCollection<Order>? _orders;
        private readonly IMongoCollection<Customer>? _customer;
        private readonly IMongoCollection<Vendor>? _vendor;
        private readonly IMongoCollection<Product>? _product;

        // Constructor: Initializes the controller with MongoDB collections for orders, customers, products, and vendors.
        public DashboardController(MongoDbService mongoDbService)
        {
            _orders = mongoDbService.Database?.GetCollection<Order>("order");
            _customer = mongoDbService.Database?.GetCollection<Customer>("customer");
            _product = mongoDbService.Database?.GetCollection<Product>("product");
            _vendor = mongoDbService.Database?.GetCollection<Vendor>("Vendors");
        }

        // Method: GetCounts
        // Purpose: Retrieves the total counts of orders, customers, products, and vendors from the database.
        // Returns: A JSON object containing the counts of each entity.
        [HttpGet("admin/dashboardcounts")]
        public async Task<IActionResult> GetCounts()
        {
            var totalOrders = await _orders.CountDocumentsAsync(FilterDefinition<Order>.Empty);
            var totalCustomers = await _customer.CountDocumentsAsync(FilterDefinition<Customer>.Empty);
            var totalProducts = await _product.CountDocumentsAsync(FilterDefinition<Product>.Empty);
            var totalVendors = await _vendor.CountDocumentsAsync(FilterDefinition<Vendor>.Empty);

            var counts = new DashboardCounts
            {
                TotalOrders = totalOrders,
                TotalCustomers = totalCustomers,
                TotalProducts = totalProducts,
                TotalVendors = totalVendors
            };

            return Ok(counts);
        }


    }
}
