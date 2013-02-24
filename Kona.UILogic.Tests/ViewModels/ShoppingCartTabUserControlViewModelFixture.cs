// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kona.UILogic.Events;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ShoppingCartTabUserControlViewModelFixture
    {
        [TestMethod]
        public void NavigatedTo_CalculatesTotalNumberOfItemsInCart()
        {
            var shoppingCart = new ShoppingCart(new List<ShoppingCartItem>()
                                               {
                                                   new ShoppingCartItem {Quantity = 1},
                                                   new ShoppingCartItem {Quantity = 2}
                                               });
            var shoppingCartRepository = new MockShoppingCartRepository();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type =>
            {
                if (type == typeof(ShoppingCartUpdatedEvent)) return new ShoppingCartUpdatedEvent();
                if (type == typeof(ShoppingCartItemUpdatedEvent)) return new ShoppingCartItemUpdatedEvent();
                return null;
            };
            var accountService = new MockAccountService();
            accountService.GetSignedInUserAsyncDelegate = () => Task.FromResult((UserInfo) null);
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, null, new AlertMessageService(), null, accountService);

            Assert.AreEqual(3, target.ItemCount);
        }

        [TestMethod]
        public void VMListensToShoppingCartUpdatedEvent_ThenCalculatesTotalNumberOfItemsInCart()
        {
            var shoppingCart = new ShoppingCart(new List<ShoppingCartItem>());
            var shoppingCartRepository = new MockShoppingCartRepository();

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var shoppingCartUpdatedEvent = new ShoppingCartUpdatedEvent();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type =>
            {
                if (type == typeof(ShoppingCartUpdatedEvent)) return shoppingCartUpdatedEvent;
                if (type == typeof(ShoppingCartItemUpdatedEvent)) return new ShoppingCartItemUpdatedEvent();
                return null;
            };
            var accountService = new MockAccountService();
            accountService.GetSignedInUserAsyncDelegate = () => Task.FromResult((UserInfo)null);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, null, new AlertMessageService(), null, accountService);

            shoppingCart = new ShoppingCart(new List<ShoppingCartItem>()
                                               {
                                                   new ShoppingCartItem {Quantity = 1},
                                                   new ShoppingCartItem {Quantity = 2}
                                               });

            Assert.AreEqual(0, target.ItemCount);

            shoppingCartUpdatedEvent.Publish(null);

            Assert.AreEqual(3, target.ItemCount);

        }

        [TestMethod]
        public void ExecutingShoppingCartCommand_NavigatesToShoppingCart()
        {
            var navigateCalled = false;
            var navigationService = new MockNavigationService();
            navigationService.NavigateDelegate = (s, o) =>
                                                     {
                                                         Assert.AreEqual("ShoppingCart", s);
                                                         navigateCalled = true;
                                                         return true;
                                                     };
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type =>
            {
                if (type == typeof(ShoppingCartUpdatedEvent)) return new ShoppingCartUpdatedEvent();
                if (type == typeof(ShoppingCartItemUpdatedEvent)) return new ShoppingCartItemUpdatedEvent();
                return null;
            };
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(new ShoppingCart(null));
            var accountService = new MockAccountService();
            accountService.GetSignedInUserAsyncDelegate = () => Task.FromResult((UserInfo)null);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, navigationService, new AlertMessageService(), null, accountService);
            target.ShoppingCartTabCommand.Execute();

            Assert.IsTrue(navigateCalled);
        }

        [TestMethod]
        public void ShoppingCartUpdated_WithNullCart_SetsItemCountZero()
        {
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult<ShoppingCart>(null);
            var eventAggregator = new MockEventAggregator();
            var shoppingCartUpdatedEvent = new ShoppingCartUpdatedEvent();
            eventAggregator.GetEventDelegate = type =>
            {
                if (type == typeof(ShoppingCartUpdatedEvent)) return shoppingCartUpdatedEvent;
                if (type == typeof(ShoppingCartItemUpdatedEvent)) return new ShoppingCartItemUpdatedEvent();
                return null;
            };
            var accountService = new MockAccountService();
            accountService.GetSignedInUserAsyncDelegate = () => Task.FromResult((UserInfo)null);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, null, new AlertMessageService(), null, accountService);
            target.ItemCount = 99;

            shoppingCartUpdatedEvent.Publish(null);

            Assert.AreEqual(0, target.ItemCount);
        }

        [TestMethod]
        public void FailedCallToShoppingCartRepository_ShowsAlert()
        {
            var alertCalled = false;
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
                                                                      {
                                                                          throw new HttpRequestException();
                                                                      };
            var alertMessageService = new MockAlertMessageService();
            alertMessageService.ShowAsyncDelegate = (s, s1) =>
                                                        {
                                                            alertCalled = true;
                                                            Assert.AreEqual("Error", s1);
                                                            return Task.FromResult(string.Empty);
                                                        };
            var accountService = new MockAccountService();
            accountService.GetSignedInUserAsyncDelegate = () => Task.FromResult((UserInfo)null);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, null, null,
                                                                 alertMessageService, new MockResourceLoader(), accountService);

            Assert.IsTrue(alertCalled);
        }
    }
}
