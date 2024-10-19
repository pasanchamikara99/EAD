using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using E_commerce_system.Entities;
using E_commerce_system.Repositories;

namespace E_commerce_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound();
            }
            return cart;
        }

        [HttpPost("{userId}/items")]
        public async Task<ActionResult<Cart>> AddToCart(string userId, CartItem item)
        {
            var updatedCart = await _cartRepository.AddToCart(userId, item);
            return CreatedAtAction(nameof(GetCart), new { userId = userId }, updatedCart);
        }

        [HttpPut("{userId}/items")]
        public async Task<ActionResult<Cart>> UpdateCartItem(string userId, CartItem item)
        {
            var updatedCart = await _cartRepository.UpdateCartItem(userId, item);
            if (updatedCart == null)
            {
                return NotFound();
            }
            return updatedCart;
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveFromCart(string userId, string productId)
        {
            var result = await _cartRepository.RemoveFromCart(userId, productId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> ClearCart(string userId)
        {
            var result = await _cartRepository.ClearCart(userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}