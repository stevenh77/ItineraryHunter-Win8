// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
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
    public class ShoppingCartPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_Fill_Properties_No_Shopping_Cart_Items()
        {
            var navigationService = new MockNavigationService();
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
            {
                var shoppingCartItems = new List<ShoppingCartItem>();
                shoppingCartItems.Add(new ShoppingCartItem { Quantity = 1, Product = new Product { ListPrice = 200}, Currency = "USD" });
                ShoppingCart shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>(shoppingCartItems)) { Currency = "USD" };
                return Task.FromResult(shoppingCart);
            };
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => new MockShoppingCartUpdatedEvent();

            var target = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, new MockAccountService(), null, null, null, null, null, eventAggregator);
            target.OnNavigatedTo(null, NavigationMode.New, null);

            Assert.AreEqual("$200.00", target.FullPrice);
            Assert.AreEqual("$100.00", target.TotalDiscount);
            Assert.AreEqual(1, target.ShoppingCartItemViewModels.Count);
        }

        [TestMethod]
        public void OnNavigatedTo_Fill_Properties_With_Shopping_Cart_Items()
        {
            var navigationService = new MockNavigationService();
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
            {
                ShoppingCart shoppingCart = null;
                var shoppingCartItems = new ObservableCollection<ShoppingCartItem>
                                            {
                                                new ShoppingCartItem() {Product = new Product { ListPrice = 100, ProductNumber = "p1", ImageUri = new Uri("http://image")}, Currency = "USD", Quantity = 1}, 
                                                new ShoppingCartItem() {Product = new Product { ListPrice = 100, ProductNumber = "p2", ImageUri = new Uri("http://image")}, Currency = "USD", Quantity = 1}
                                            };
                shoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD"};

                return Task.FromResult(shoppingCart);
            };
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => new MockShoppingCartUpdatedEvent();
            var target = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, new MockAccountService(), null, null, null, null, null, eventAggregator);
            target.OnNavigatedTo(null, NavigationMode.New, null);

            Assert.AreEqual("$200.00", target.FullPrice);
            Assert.AreEqual("$100.00", target.TotalDiscount);
            Assert.AreEqual(2, target.ShoppingCartItemViewModels.Count);
        }

        [TestMethod]
        public void ShoppingCartUpdated_WhenShoppingCartChanged()
        {
            var navigationService = new MockNavigationService();
            var accountService = new MockAccountService();
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
            {
                ShoppingCart shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>()) { Currency = "USD"};
                return Task.FromResult(shoppingCart);
            };
            var eventAggregator = new MockEventAggregator();
            var shoppingCartUpdatedEvent = new ShoppingCartUpdatedEvent();
            eventAggregator.GetEventDelegate = type => shoppingCartUpdatedEvent;
            var target = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, accountService, null, null, null, null, null, eventAggregator);
            target.OnNavigatedTo(null, NavigationMode.New, null);  

            Assert.AreEqual("$0.00", target.FullPrice);

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
            {
                var shoppingCartItems = new ObservableCollection<ShoppingCartItem>
                                            {
                                                new ShoppingCartItem() { Product = new Product { ListPrice = 100, ProductNumber = "p1", ImageUri = new Uri("http://image")}, Currency = "USD", Quantity = 2}, 
                                            };
                ShoppingCart shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>(shoppingCartItems)) { Currency = "USD" };
                return Task.FromResult(shoppingCart);
            };

            shoppingCartUpdatedEvent.Publish(null);

            Assert.AreEqual("$200.00", target.FullPrice);

        }

        [TestMethod]
        public void UpdateShoppingCart_ClearsFields_WhenShoppingCartEmpty()
        {
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult<ShoppingCart>(null);
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => new MockShoppingCartUpdatedEvent();
            var target = new ShoppingCartPageViewModel(shoppingCartRepository, new MockNavigationService(),
                                                       new MockAccountService(), new MockFlyoutService(), null, null, null, null, eventAggregator);

            target.OnNavigatedTo(null, NavigationMode.New, null);

            Assert.AreEqual(string.Empty, target.TotalPrice);
            Assert.AreEqual(string.Empty, target.TotalDiscount);
            Assert.AreEqual(string.Empty, target.FullPrice);
        }

        [TestMethod]
        public async Task Checkout_WhenAnonymous_ShowsSignInFlyout()
        {
            var showFlyoutCalled = false;
            var accountService = new MockAccountService
                {
                    GetSignedInUserAsyncDelegate = () => Task.FromResult<UserInfo>(null)
                };
            var flyoutService = new MockFlyoutService
                {
                    ShowFlyoutDelegate = (s, o, arg3) =>
                        {
                            showFlyoutCalled = true;
                            Assert.AreEqual("SignIn", s);
                        }
                };
            var checkoutDataRepository = new MockCheckoutDataRepository()
                {
                    GetDefaultShippingAddressDelegate = () => null,
                    GetDefaultBillingAddresDelegate =  () => null,
                    GetDefaultPaymentMethodDelegate = () => Task.FromResult<PaymentMethod>(null)
                };
            var eventAggregator = new MockEventAggregator
                {
                    GetEventDelegate = type => new MockShoppingCartUpdatedEvent()
                };
            var target = new ShoppingCartPageViewModel(null, new MockNavigationService(), accountService, flyoutService, null, null, checkoutDataRepository, null, eventAggregator);

            await target.CheckoutCommand.Execute();

            Assert.IsTrue(showFlyoutCalled);
        }

        [TestMethod]
        public void CheckoutCommand_NotExecutable_IfNoItemsInCart()
        {
            var navigationService = new MockNavigationService();
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult<ShoppingCart>(null);
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => new MockShoppingCartUpdatedEvent();
            var target = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, null, null, null, null, null, null, eventAggregator);
            target.UpdateShoppingCartAsync(null);
            
            Assert.IsFalse(target.CheckoutCommand.CanExecute());

            shoppingCartRepository.GetShoppingCartAsyncDelegate = 
                () => Task.FromResult(new ShoppingCart(new Collection<ShoppingCartItem>()){Currency = "USD", FullPrice = 0, TaxRate = 0, TotalDiscount = 0, TotalPrice = 0});
            target.UpdateShoppingCartAsync(null);
            
            Assert.IsFalse(target.CheckoutCommand.CanExecute());

            shoppingCartRepository.GetShoppingCartAsyncDelegate =
                () => Task.FromResult(new ShoppingCart(new Collection<ShoppingCartItem> { new ShoppingCartItem{Product = new Product(), Currency = "USD", Quantity = 0} }) 
                { Currency = "USD", FullPrice = 0, TaxRate = 0, TotalDiscount = 0, TotalPrice = 0 });
            target.UpdateShoppingCartAsync(null);

            Assert.IsTrue(target.CheckoutCommand.CanExecute());

        }

        [TestMethod]
        public void DecrementCountCommand_NotExecutable()
        {
            var navigationService = new MockNavigationService();
            var shoppingCartRepository = new MockShoppingCartRepository();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => new MockShoppingCartUpdatedEvent();
            var target = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, null, null, null, null, null, null, eventAggregator);

            target.SelectedItem = new ShoppingCartItemViewModel(new ShoppingCartItem(){ Quantity = 2, Currency = "USD", Product = new Product(), });

            Assert.IsTrue(target.DecrementCountCommand.CanExecute());

            target.SelectedItem = new ShoppingCartItemViewModel(new ShoppingCartItem() { Quantity = 1, Currency = "USD", Product = new Product() });

            Assert.IsFalse(target.DecrementCountCommand.CanExecute());
        }

        // Note: The remove method is a WIP
        //[TestMethod]
        //public void Remove_Item_From_Collection()
        //{
        //    var shoppingCartRepository = new MockShoppingCartRepository();
        //    var navigationService = new MockNavigationService();
        //    var accountManager = new MockAccountManager();

        //    shoppingCartRepository.GetShoppingCartAsyncDelegate = (userId) =>
        //    {
        //        ShoppingCart shoppingCart = null;
        //        var shoppingCartItems = new ObservableCollection<ShoppingCartItem> { new ShoppingCartItem() { Item = new Item() }, new ShoppingCartItem() { Item = new Item() } };
        //        shoppingCart = new ShoppingCart(shoppingCartItems) { FullPrice = 200, TotalDiscount = 100 };

        //        return Task.FromResult(shoppingCart);
        //    };

        //    shoppingCartRepository.RemoveItemFromShoppingCartAsyncDelegate = (userId, itemId)  =>
        //    {
        //        return Task.FromResult(true);
        //    };

        //    var viewModel = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, accountManager);
        //    viewModel.OnNavigatedTo("JohnDoe", NavigationMode.New, null);
        //    Assert.AreEqual(2, viewModel.ShoppingCartItemViewModels.Count);
        //    var cartItem = viewModel.ShoppingCartItemViewModels.First();
        //    viewModel.Remove(cartItem);
        //    Assert.AreEqual(1, viewModel.ShoppingCartItemViewModels.Count);
        //}

    }
}
