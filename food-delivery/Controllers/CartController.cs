using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using food_delivery.Domain;
using food_delivery.Service;
using food_delivery.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace food_delivery.Controllers
{

   [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICustomerService _customerService;
        private readonly IFoodDeliveryService _foodDeliveryservice;

        public CartController(ICartService cartService, ICustomerService customerService, IFoodDeliveryService foodDeliveryservice)
        {
            _cartService = cartService;
            _customerService = customerService;
            _foodDeliveryservice = foodDeliveryservice;
        }

        [Authorize]
        [HttpPost("addToCart")]
        public ActionResult<Cart> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            try
            {
                var updatedCart = _cartService.AddFoodToCart(addToCartDto);

                return Ok(updatedCart);
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getCart/{userName}")]
        public ActionResult<Cart> GetCart(string userName)
        {
            try
            {
                var customer = _customerService.GetCustomerByUsername(userName);
                var cart = _cartService.GetCartById(customer.Cart.CartId);

                cart.OrderItems = _cartService.GetCartItems(cart);
                return Ok(cart);
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("removeFromCart")]
        public ActionResult<Cart> RemoveFromCart([FromBody] RemoveFromCartDto addToCartDto)
        {
            try
            {
                var customer = _customerService.GetCustomerByUsername(addToCartDto.UserName);
                var cart = _cartService.GetCartById(customer.Cart.CartId);

                var food = _foodDeliveryservice.GetFoodByName(addToCartDto.FoodName);

                if (food == null)
                {
                    return NotFound("Food not found");
                }

                _cartService.RemoveFoodFromCart(cart, food);

                cart.OrderItems = _cartService.GetCartItems(cart);

                return Ok(cart);
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("updateCartItemQuantity")]
        public ActionResult<Cart> UpdateCartItemQuantity([FromBody] AddToCartDto updateCartItemQuantityDto)
        {
            try
            {
                var customer = _customerService.GetCustomerByUsername(updateCartItemQuantityDto.UserName);
                var cart = _cartService.GetCartById(customer.Cart.CartId);

                var food = _foodDeliveryservice.GetFoodByName(updateCartItemQuantityDto.FoodName);

                if (food == null)
                {
                    return NotFound("Food not found");
                }

                _cartService.UpdateCartItemQuantity(cart, food, updateCartItemQuantityDto.Quantity);

                cart.OrderItems = _cartService.GetCartItems(cart);

                return Ok(cart);
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("clearCart/{userName}")]
        public ActionResult<string> ClearCart(string userName)
        {
            _cartService.ClearCart(userName);
            return Ok("Cart of user "+ userName +" is empty now");
        }
    }
}