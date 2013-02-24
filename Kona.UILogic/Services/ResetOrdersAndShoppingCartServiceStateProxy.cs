// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kona.UILogic.Services
{
    public class ResetOrdersAndShoppingCartServiceStateProxy
    {
        private string _shoppingCartBaseUrl = string.Format("{0}/api/ShoppingCart/", Constants.ServerAddress);
        private string _ordersBaseUrl = string.Format("{0}/api/Order/", Constants.ServerAddress);

        public async Task Reset()
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(_shoppingCartBaseUrl + "?reset=true",null);
                response.EnsureSuccessStatusCode();
                response = await client.PostAsync(_ordersBaseUrl + "?reset=true", null);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
