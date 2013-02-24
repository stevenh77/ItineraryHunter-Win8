// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Web.Http;
using Kona.WebServices.Models;
using Kona.WebServices.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Kona.WebServices.Controllers
{
    public class ShoppingCartController : ApiController
    {
        private IShoppingCartRepository _shoppingCartRepository;
        private IProductRepository _productRepository;
        // Need to lock to prevent concurrent shopping cart modifications because of using static memory for carts in the repositories
        private static object _lock = new object();

        public ShoppingCartController()
            : this(new ShoppingCartRepository(), new ProductRepository())
        { }

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        // GET /api/ShoppingCart/id
        public ShoppingCart Get(string id)
        {
            return _shoppingCartRepository.GetById(id);
        }

        // POST /api/ShoppingCart/{id}?productIdToIncrement={productIdToIncrement}
        [HttpPost]
        public void AddProductToShoppingCart(string id, string productIdToIncrement)
        {
            lock (_lock)
            {
                var product = _productRepository.GetAll().FirstOrDefault(c => c.ProductNumber == productIdToIncrement);
                if (product == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                _shoppingCartRepository.AddProductToCart(id, product);
            }
        }

        // POST /api/ShoppingCart/{id}?productIdToIncrement={productIdToIncrement}
        //This action decrements the quantity of an item in the shopping cart. 
        //For example, if a shopping cart has 3 socks, this action will update the quantity to 2.
        [HttpPost]
        public void RemoveProductFromShoppingCart(string id, string productIdToDecrement)
        {
            lock (_lock)
            {
                var product = _productRepository.GetAll().FirstOrDefault(c => c.ProductNumber == productIdToDecrement);
                if (product == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                if (!_shoppingCartRepository.RemoveProductFromCart(id, productIdToDecrement))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
        }

        // DELETE /api/ShoppingCart/{id}
        public void DeleteShoppingCart(string id)
        {
            if (!_shoppingCartRepository.Delete(id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // PUT /api/ShoppingCart/{id}?itemIdToRemove={itemIdToRemove}
        //This action removes the entire item group from the shopping cart. 
        //For example, if a shopping cart has 2 socks and 3 bikes, this action will remove socks.
        [HttpPut]
        public void RemoveShoppingCartItem(string id, string itemIdToRemove)
        {
            lock (_lock)
            {
                ShoppingCart shoppingCart = _shoppingCartRepository.GetById(id);

                if (shoppingCart == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                if (!_shoppingCartRepository.RemoveItemFromCart(shoppingCart, itemIdToRemove))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
        }

        // POST /api/ShoppingCart/{id}?oldShoppingCartId={oldShoppingCartId}
        [HttpPost]
        public HttpResponseMessage MergeShoppingCarts(string id, string oldShoppingCartId)
        {
            lock (_lock)
            {
                if (id == oldShoppingCartId) return new HttpResponseMessage(HttpStatusCode.OK);

                var oldCart = _shoppingCartRepository.GetById(oldShoppingCartId);

                if (oldCart != null)
                {
                    //Merge shopping carts by adding items from old cart to new cart.
                    foreach (var shoppingCartItem in oldCart.ShoppingCartItems)
                    {
                        for (int i = 0; i < shoppingCartItem.Quantity; i++)
                        {
                            _shoppingCartRepository.AddProductToCart(id, shoppingCartItem.Product);
                        }
                    }

                    //Delete old cart
                    _shoppingCartRepository.Delete(oldShoppingCartId);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        [HttpPost]
        public void Reset(bool reset)
        {
            if (reset)
                ShoppingCartRepository.Reset();
        }
    }
}
