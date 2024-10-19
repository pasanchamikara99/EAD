using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using E_commerce_system.Entities;

namespace E_commerce_system.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserId(string userId);
        Task<Cart> AddToCart(string userId, CartItem item);
        Task<Cart> UpdateCartItem(string userId, CartItem item);
        Task<bool> RemoveFromCart(string userId, string productId);
        Task<bool> ClearCart(string userId);
    }

    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> _carts;

        public CartRepository(IMongoDatabase database)
        {
            _carts = database.GetCollection<Cart>("Carts");
        }

        public async Task<Cart> GetCartByUserId(string userId)
        {
            return await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Cart> AddToCart(string userId, CartItem item)
        {
            var cart = await GetCartByUserId(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem> { item },
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _carts.InsertOneAsync(cart);
            }
            else
            {
                var existingItem = cart.Items.Find(i => i.ProductId == item.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    cart.Items.Add(item);
                }
                cart.UpdatedAt = DateTime.UtcNow;
                await _carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            }
            return cart;
        }

        public async Task<Cart> UpdateCartItem(string userId, CartItem item)
        {
            var cart = await GetCartByUserId(userId);
            if (cart != null)
            {
                var existingItem = cart.Items.Find(i => i.ProductId == item.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity = item.Quantity;
                    existingItem.Price = item.Price;
                    cart.UpdatedAt = DateTime.UtcNow;
                    await _carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
                }
            }
            return cart;
        }

        public async Task<bool> RemoveFromCart(string userId, string productId)
        {
            var cart = await GetCartByUserId(userId);
            if (cart != null)
            {
                cart.Items.RemoveAll(i => i.ProductId == productId);
                cart.UpdatedAt = DateTime.UtcNow;
                var result = await _carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
                return result.ModifiedCount > 0;
            }
            return false;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var result = await _carts.DeleteOneAsync(c => c.UserId == userId);
            return result.DeletedCount > 0;
        }
    }
}