// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.WebServices.Models;
using Kona.WebServices.Repositories;

namespace Kona.WebServices.Tests.Mocks
{
    public class MockShoppingCartRepository : IShoppingCartRepository
    {
        public Func<string, ShoppingCart> GetByIdDelegate { get; set; }
        public Func<string, bool> DeleteDelegate { get; set; }
        public Func<string, Product, ShoppingCartItem> AddProductToCartDelegate { get; set; }
        public Func<string, string, bool> RemoveProductFromCartDelegate { get; set; }
        public Func<ShoppingCart, string, bool> RemoveItemFromCartDelegate { get; set; }
        

        ShoppingCart IShoppingCartRepository.GetById(string shoppingCartId)
        {
            return GetByIdDelegate(shoppingCartId);
        }

        bool IShoppingCartRepository.Delete(string userId)
        {
            return DeleteDelegate(userId);
        }

        ShoppingCartItem IShoppingCartRepository.AddProductToCart(string shoppingCartId, Product product)
        {
            return AddProductToCartDelegate(shoppingCartId, product);
        }

        bool IShoppingCartRepository.RemoveProductFromCart(string shoppingCartId, string productId)
        {
            return RemoveProductFromCartDelegate(shoppingCartId, productId);
        }

        bool IShoppingCartRepository.RemoveItemFromCart(ShoppingCart shoppingCart, string itemId)
        {
            return RemoveItemFromCartDelegate(shoppingCart, itemId);
        }
    }
}
