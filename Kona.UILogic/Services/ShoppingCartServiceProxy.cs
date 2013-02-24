// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Net.Http;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;

namespace Kona.UILogic.Services
{
    public class ShoppingCartServiceProxy : IShoppingCartService
    {
        private string _shoppingCartBaseUrl = string.Format("{0}/api/ShoppingCart/", Constants.ServerAddress);

        public async Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                shoppingCartClient.AddCurrentCultureHeader();
                var response = await shoppingCartClient.GetAsync(_shoppingCartBaseUrl + shoppingCartId);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ShoppingCart>();

                return result;
            }
        }

        public async Task AddProductToShoppingCartAsync(string shoppingCartId, string productIdToIncrement)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                shoppingCartClient.AddCurrentCultureHeader();
                string requestUrl = _shoppingCartBaseUrl + shoppingCartId + "?productIdToIncrement=" + productIdToIncrement;
                var response = await shoppingCartClient.PostAsync(requestUrl, null);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task RemoveProductFromShoppingCartAsync(string shoppingCartId, string productIdToDecrement)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                shoppingCartClient.AddCurrentCultureHeader();
                string requestUrl = _shoppingCartBaseUrl + shoppingCartId + "?productIdToDecrement=" + productIdToDecrement;
                var response = await shoppingCartClient.PostAsync(requestUrl, null);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task RemoveShoppingCartItemAsync(string shoppingCartId, string itemIdToRemove)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                shoppingCartClient.AddCurrentCultureHeader();
                string requestUrl = _shoppingCartBaseUrl + shoppingCartId + "?itemIdToRemove=" + itemIdToRemove;
                var response = await shoppingCartClient.PutAsync(requestUrl, null);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeleteShoppingCartAsync(string shoppingCartId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                shoppingCartClient.AddCurrentCultureHeader();
                var response = await shoppingCartClient.DeleteAsync(_shoppingCartBaseUrl + shoppingCartId);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task MergeShoppingCartsAsync(string oldShoppingCartId, string newShoppingCartId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                shoppingCartClient.AddCurrentCultureHeader();
                string requestUrl = _shoppingCartBaseUrl + newShoppingCartId + "?oldShoppingCartId=" + oldShoppingCartId;
                var response = await shoppingCartClient.PostAsync(requestUrl, null);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
