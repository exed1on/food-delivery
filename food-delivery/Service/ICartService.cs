﻿using food_delivery.Domain;
using food_delivery.Dto;

namespace food_delivery.Service
{
    public interface ICartService
    {
        Cart AddFoodToCart(AddToCartDto addToCartDto);
        void RemoveFoodFromCart(Cart cart, Food food);
        List<OrderItem> GetCartItems(Cart cart);
        void UpdateCartItemQuantity(Cart cart, Food food, int newQuantity);
        void ClearCart(string userName);
        Cart GetCartById(long cartId);
        Food GetFoodById(long foodId);
    }
}