using MongoDB.Driver;
using E_commerce_system.Data.Entities;

namespace E_commerce_system.Data.Services
{
    public interface IVendorService
    {
        Task<List<Vendor>> GetAllVendorsAsync();
        Task<Vendor> GetVendorByIdAsync(string id);
        Task<Vendor> CreateVendorAsync(Vendor vendor);
        Task<bool> UpdateVendorAsync(string id, Vendor vendor);
        Task<bool> DeleteVendorAsync(string id);
        Task<bool> AddRatingAsync(string vendorId, VendorRating rating);
        Task<bool> UpdateCommentAsync(string vendorId, string customerId, string newComment);
        Task<List<VendorRating>> GetVendorRatingsAsync(string vendorId);
        Task<bool> IsEmailInUseAsync(string email);

        Task<Vendor> GetVendorByEmail(string email);
    }

    public class VendorService : IVendorService
    {
        private readonly IMongoCollection<Vendor> _vendors;

        public VendorService(IMongoDatabase database)
        {
            _vendors = database.GetCollection<Vendor>("Vendors");
        }

        public async Task<List<Vendor>> GetAllVendorsAsync()
        {
            return await _vendors.Find(vendor => true).ToListAsync();
        }

        public async Task<Vendor> GetVendorByIdAsync(string id)
        {
            return await _vendors.Find(v => v.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Vendor> CreateVendorAsync(Vendor vendor)
        {
            await _vendors.InsertOneAsync(vendor);
            return vendor;
        }

        public async Task<bool> UpdateVendorAsync(string id, Vendor vendor)
        {
            var result = await _vendors.ReplaceOneAsync(v => v.Id == id, vendor);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteVendorAsync(string id)
        {
            var result = await _vendors.DeleteOneAsync(v => v.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> AddRatingAsync(string vendorId, VendorRating rating)
        {
            var update = Builders<Vendor>.Update.Push(v => v.Ratings, rating);
            var result = await _vendors.UpdateOneAsync(v => v.Id == vendorId, update);
            return result.ModifiedCount > 0;
        }


        public async Task<bool> UpdateCommentAsync(string vendorId, string customerId, string newComment)
        {
            var vendor = await GetVendorByIdAsync(vendorId);
            if (vendor == null) return false;

            var rating = vendor.Ratings.FirstOrDefault(r => r.CustomerId == customerId);
            if (rating == null) return false;

            rating.Comment = newComment;
            rating.ModifiedDate = DateTime.UtcNow;

            var update = Builders<Vendor>.Update.Set(v => v.Ratings, vendor.Ratings);
            var result = await _vendors.UpdateOneAsync(v => v.Id == vendorId, update);
            return result.ModifiedCount > 0;
        }

        public async Task<List<VendorRating>> GetVendorRatingsAsync(string vendorId)
        {
            var vendor = await GetVendorByIdAsync(vendorId);
            return vendor?.Ratings ?? new List<VendorRating>();
        }

        public async Task<bool> IsEmailInUseAsync(string email)
        {
            return await _vendors.Find(v => v.Email == email).AnyAsync();
        }

        public async Task<Vendor> GetVendorByEmail(string email)
        {
            return await _vendors.Find(v => v.Email == email).FirstOrDefaultAsync();
        }
    }
}