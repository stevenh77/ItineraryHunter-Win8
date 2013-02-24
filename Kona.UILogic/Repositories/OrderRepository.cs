// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kona.Infrastructure.Interfaces;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IShippingMethodService _shippingMethodService;
        private readonly ISuspensionManagerState _suspensionManagerState;
        private Order _currentOrder;
        private const string OrderKey = "CurrentOrderKey";

        public OrderRepository(IOrderService orderService, IAccountService accountService, IShippingMethodService shippingMethodService, ISuspensionManagerState suspensionManagerState)
        {
            _orderService = orderService;
            _accountService = accountService;
            _shippingMethodService = shippingMethodService;
            _suspensionManagerState = suspensionManagerState;
        }

        public async Task CreateBasicOrderAsync(string userId, ShoppingCart shoppingCart, Address shippingAddress, Address billingAddress, PaymentMethod paymentMethod)
        {
            var basicShippingMethod = await _shippingMethodService.GetBasicShippingMethodAsync();
            _currentOrder = await CreateOrderAsync(userId, shoppingCart, shippingAddress, billingAddress, paymentMethod, basicShippingMethod);
            _suspensionManagerState.SessionState[OrderKey] = _currentOrder;
        }

        public async Task<Order> CreateOrderAsync(string userId, ShoppingCart shoppingCart, Address shippingAddress,
                                                  Address billingAddress, PaymentMethod paymentMethod, ShippingMethod shippingMethod)
        {
            Order order = new Order
                {
                    UserId = userId,
                    ShoppingCart = shoppingCart,
                    ShippingAddress = shippingAddress,
                    BillingAddress = billingAddress,
                    PaymentMethod = paymentMethod,
                    ShippingMethod = shippingMethod
                };

            order.Id = await _orderService.CreateOrderAsync(order, _accountService.ServerCookieHeader);

            return order;
        }

        public Order GetCurrentOrder()
        {
            if (_currentOrder != null)
            {
                return _currentOrder;
            }

            var order = _suspensionManagerState.SessionState[OrderKey] as Order;
            return order;
        }
    }
}
