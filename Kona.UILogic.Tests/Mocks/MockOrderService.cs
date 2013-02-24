// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockOrderService : IOrderService
    {
        public Func<Order, string, Task<int>> CreateOrderAsyncDelegate { get; set; }
        public Func<Order, string, Task<Order>> UpdateOrderAsyncDelegate { get; set; }
        public Func<Order, string, Task> ProcessOrderAsyncDelegate { get; set; }

        public Task<int> CreateOrderAsync(Order order, string serverCookieHeader)
        {
            return CreateOrderAsyncDelegate(order, serverCookieHeader);
        }

        public Task<Order> UpdateOrderAsync(Order order, string serverCookieHeader)
        {
            return UpdateOrderAsyncDelegate(order, serverCookieHeader);
        }

        public Task ProcessOrderAsync(Order order, string serverCookieHeader)
        {
            return ProcessOrderAsyncDelegate(order, serverCookieHeader);
        }
    }
}
