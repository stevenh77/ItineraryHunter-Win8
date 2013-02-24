// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;

namespace Kona.UILogic.Services
{
    public class OrderServiceProxy : IOrderService
    {
        private string _clientBaseUrl = string.Format("{0}/api/Order/", Constants.ServerAddress);

        public async Task<int> CreateOrderAsync(Order order, string serverCookieHeader)
        {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var orderClient = new HttpClient(handler))
                {
                    orderClient.AddCurrentCultureHeader();
                    Uri serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, serverCookieHeader);
                    orderClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    string requestUrl = _clientBaseUrl;
                    var response = await orderClient.PostAsJsonAsync<Order>(requestUrl, order);
                    await response.EnsureSuccessWithValidationSupportAsync();
                    return await response.Content.ReadAsAsync<int>();
                }
            }
        }

        public async Task ProcessOrderAsync(Order order, string serverCookieHeader)
        {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var orderClient = new HttpClient(handler))
                {
                    orderClient.AddCurrentCultureHeader();
                    Uri serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, serverCookieHeader);
                    orderClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    string requestUrl = _clientBaseUrl + order.Id;
                    var response = await orderClient.PutAsJsonAsync<Order>(requestUrl, order);
                    await response.EnsureSuccessWithValidationSupportAsync();
                }
            }
        }
    }
}
