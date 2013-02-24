// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Kona.Infrastructure.Interfaces;
using Kona.UILogic.Events;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Microsoft.Practices.PubSubEvents;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Kona.UILogic.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public const string ShoppingCartIdKey = "ShoppingCartId";
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAccountService _accountService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly ISuspensionManagerState _suspensionManagerState;

        private string _shoppingCartId;
        private ShoppingCart _cachedShoppingCart = null;
        
        public ShoppingCartRepository(IShoppingCartService shoppingCartService, IAccountService accountService, IEventAggregator eventAggregator, IProductCatalogRepository productCatalogRepository, ISuspensionManagerState suspensionManagerState)
        {
            _shoppingCartService = shoppingCartService;
            _accountService = accountService;
            _eventAggregator = eventAggregator;
            _productCatalogRepository = productCatalogRepository;
            _suspensionManagerState = suspensionManagerState;

            if (accountService != null)
            {
                _accountService.UserChanged += _accountService_UserChanged;
            }

            if (_suspensionManagerState != null && _suspensionManagerState.SessionState.ContainsKey(ShoppingCartIdKey))
            {
                _shoppingCartId = _suspensionManagerState.SessionState[ShoppingCartIdKey].ToString();
            }
            else
            {
                _shoppingCartId = Guid.NewGuid().ToString();
                _suspensionManagerState.SessionState[ShoppingCartIdKey] = _shoppingCartId;
            }
        }

        public async Task ClearCartAsync()
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.DeleteShoppingCartAsync(_shoppingCartId);
            RaiseShoppingCartUpdated();
        }

        async void _accountService_UserChanged(object sender, UserChangedEventArgs e)
        {
            _cachedShoppingCart = null;
            if (e.NewUserInfo != null)
            {
                if (e.OldUserInfo == null)
                {
                    await _shoppingCartService.MergeShoppingCartsAsync(_shoppingCartId, e.NewUserInfo.UserName);
                }
                _shoppingCartId = e.NewUserInfo.UserName;                
            }
            else
            {
                _shoppingCartId = Guid.NewGuid().ToString();
            }

            _suspensionManagerState.SessionState[ShoppingCartIdKey] = _shoppingCartId;
            RaiseShoppingCartUpdated();
        }

        public async Task<ShoppingCart> GetShoppingCartAsync()
        {
            if (_cachedShoppingCart != null) return _cachedShoppingCart;

            ShoppingCart cart = await _shoppingCartService.GetShoppingCartAsync(_shoppingCartId);
            if (cart == null) return null;

            foreach (var shoppingCartItem in cart.ShoppingCartItems)
            {
                //Update ImageName with path to local instance of image.
                var product = await _productCatalogRepository.GetProductAsync(shoppingCartItem.Product.ProductNumber);
                shoppingCartItem.Product.ImageUri = product.ImageUri;
            }
            _cachedShoppingCart = cart;
            return cart;
        }

        public async Task AddProductToShoppingCartAsync(string productId)
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.AddProductToShoppingCartAsync(_shoppingCartId, productId);
            RaiseShoppingCartItemUpdated();
        }

        public async Task RemoveProductFromShoppingCartAsync(string productId)
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.RemoveProductFromShoppingCartAsync(_shoppingCartId, productId);
            RaiseShoppingCartItemUpdated();
        }

        public async Task RemoveShoppingCartItemAsync(string itemId)
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.RemoveShoppingCartItemAsync(_shoppingCartId, itemId);
            RaiseShoppingCartItemUpdated();
        }

        // <snippet1501>
        private void RaiseShoppingCartUpdated()
        {
            _eventAggregator.GetEvent<ShoppingCartUpdatedEvent>().Publish(null);
        }
        // </snippet1501>

        private void RaiseShoppingCartItemUpdated()
        {
            _eventAggregator.GetEvent<ShoppingCartItemUpdatedEvent>().Publish(null);
        }
    }
}
