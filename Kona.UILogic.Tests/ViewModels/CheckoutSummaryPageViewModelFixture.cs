// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class CheckoutSummaryPageViewModelFixture
    {

        [TestMethod]
        public async Task SubmitValidOrder_CallsSuccessDialog()
        {
            bool successDialogCalled = false;
            bool errorDialogCalled = false;
            var navigationService = new MockNavigationService();
            var accountService = new MockAccountService()
                {
                    GetSignedInUserAsyncDelegate = () => Task.FromResult<UserInfo>(new UserInfo())
                };
            var orderService = new MockOrderService()
                {
                    // the order is valid, it can be processed
                    ProcessOrderAsyncDelegate = (a, b) => Task.FromResult(true)
                };
            var resourcesService = new MockResourceLoader()
                {
                    GetStringDelegate = (key) => key
                };
            var alertService = new MockAlertMessageService()
                {
                    ShowAsyncWithCommandsDelegate = (dialogTitle, dialogMessage, dialogCommands) =>
                        {
                            successDialogCalled = dialogTitle.ToLower().Contains("purchased");
                            errorDialogCalled = !successDialogCalled;
                            return Task.FromResult(successDialogCalled);
        }
                };

            var target = new CheckoutSummaryPageViewModel(navigationService, orderService, null, null, null, null, accountService, null, resourcesService, alertService);
            await target.SubmitCommand.Execute();

            Assert.IsTrue(successDialogCalled);
            Assert.IsFalse(errorDialogCalled);
        }

        [TestMethod]
        public async Task SubmitInvalidOrder_CallsErrorDialog()
        {
            bool successDialogCalled = false;
            bool errorDialogCalled = false;
            var navigationService = new MockNavigationService();
            var accountService = new MockAccountService()
                {
                    GetSignedInUserAsyncDelegate = () => Task.FromResult<UserInfo>(new UserInfo())
                };
            var orderService = new MockOrderService()
                {
                    // the order is invalid, it cannot be processed
                    ProcessOrderAsyncDelegate = (a, b) =>
                        {
                            var modelValidationResult = new ModelValidationResult();
                            modelValidationResult.ModelState.Add("someKey", new List<string>() { "the value of someKey is invalid" });
                            throw new ModelValidationException(modelValidationResult);
                        }
                };
            var resourcesService = new MockResourceLoader()
                {
                    GetStringDelegate = (key) => key
                };
            var alertService = new MockAlertMessageService()
                {
                    ShowAsyncDelegate = (dialogTitle, dialogMessage) =>
        {
                        successDialogCalled = dialogTitle.ToLower().Contains("purchased");
                        errorDialogCalled = !successDialogCalled;
                        return Task.FromResult(successDialogCalled);
                    }
                };

            var target = new CheckoutSummaryPageViewModel(navigationService, orderService, null, null, null, null, accountService, null, resourcesService, alertService);
            await target.SubmitCommand.Execute();

            Assert.IsFalse(successDialogCalled);
            Assert.IsTrue(errorDialogCalled);
        }

        [TestMethod]
        public async Task Submit_WhenAnonymous_ShowsSignInFlyout()
        {
            bool showFlyoutCalled = false;
            var accountService = new MockAccountService()
                {
                    GetSignedInUserAsyncDelegate = () => Task.FromResult<UserInfo>(null)
                };
            var flyoutService = new MockFlyoutService()
        {
                    ShowFlyoutDelegate = (s, o, arg3) =>
            {
                showFlyoutCalled = true;
                Assert.AreEqual("SignIn", s);
                        }
            };

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), null, null, null, null, null, accountService, flyoutService, null, null);
            await target.SubmitCommand.Execute();

            Assert.IsTrue(showFlyoutCalled);
        }

        [TestMethod]
        public void SelectShippingMethod_Recalculates_Order()
        {
            var shippingMethods = new List<ShippingMethod>() { new ShippingMethod() { Id = 1, Cost = 0 } };
            var shoppingCartItems = new List<ShoppingCartItem>() { new ShoppingCartItem() { Quantity = 1, Currency = "USD", Product = new Product() } };
            var order = new Order()
            {
                ShoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD", TotalPrice = 100 },
                ShippingAddress = new Address(),
                BillingAddress = new Address(),
                PaymentMethod = new PaymentMethod() { CardNumber = "1234" },
                ShippingMethod = shippingMethods.First()
            };
            var shippingMethodService = new MockShippingMethodService() 
            {
                GetShippingMethodsAsyncDelegate = () => Task.FromResult<IEnumerable<ShippingMethod>>(shippingMethods) 
            };
            var orderRepository = new MockOrderRepository() { GetCurrentOrderAsyncDelegate = () => order };
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(order.ShoppingCart);

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), new MockOrderService(), orderRepository, shippingMethodService,
                                                          null, shoppingCartRepository,
                                                          new MockAccountService(), new MockFlyoutService(), new MockResourceLoader(), null);

            target.OnNavigatedTo(null, NavigationMode.New, null);

            Assert.AreEqual("$0.00", target.ShippingCost);
            Assert.AreEqual("$100.00", target.OrderSubtotal);
            Assert.AreEqual("$100.00", target.GrandTotal);

            target.SelectedShippingMethod = new ShippingMethod() { Cost = 10 };

            Assert.AreEqual("$10.00", target.ShippingCost);
            Assert.AreEqual("$100.00", target.OrderSubtotal);
            Assert.AreEqual("$110.00", target.GrandTotal);

        }

        [TestMethod]
        public void SelectCheckoutData_Opens_AppBar()
        {
            var shippingMethods = new List<ShippingMethod>() { new ShippingMethod() { Id = 1, Cost = 0 } };
            var shoppingCartItems = new List<ShoppingCartItem>() { new ShoppingCartItem() { Quantity = 1, Currency = "USD", Product = new Product() } };
            var order = new Order()
            {
                ShoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD", FullPrice = 100 },
                ShippingAddress = new Address(),
                BillingAddress = new Address(),
                PaymentMethod = new PaymentMethod() { CardNumber = "1234" },
                ShippingMethod = shippingMethods.First()
            };
            var shippingMethodService = new MockShippingMethodService()
            {
                GetShippingMethodsAsyncDelegate = () => Task.FromResult<IEnumerable<ShippingMethod>>(shippingMethods)
            };
            var orderRepository = new MockOrderRepository() {GetCurrentOrderAsyncDelegate = () => order};
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(order.ShoppingCart);

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), new MockOrderService(), orderRepository, shippingMethodService,
                                                          null, shoppingCartRepository,
                                                          new MockAccountService(), new MockFlyoutService(), new MockResourceLoader(), null);

            target.OnNavigatedTo(null, NavigationMode.New, null);
            Assert.IsFalse(target.IsBottomAppBarOpened);

            target.SelectedCheckoutData = target.CheckoutDataViewModels.First();
            Assert.IsTrue(target.IsBottomAppBarOpened);
        }

        [TestMethod]
        public void EditCheckoutData_Calls_Proper_Flyout()
        {
            string requestedFlyoutName = string.Empty;
            var flyoutService = new MockFlyoutService() { ShowFlyoutDelegate = (flyoutName, a, b) => Assert.IsTrue(flyoutName == requestedFlyoutName) };

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), null, null, null, null, null, null, flyoutService, null, null);

            requestedFlyoutName = "ShippingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.ShippingAddress, null);
            target.EditCheckoutDataCommand.Execute().Wait();

            requestedFlyoutName = "BillingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.BillingAddress, null);
            target.EditCheckoutDataCommand.Execute().Wait();

            requestedFlyoutName = "PaymentMethod";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.PaymentMethod, null);
            target.EditCheckoutDataCommand.Execute().Wait();
        }

        [TestMethod]
        public void EditCheckoutData_Updates_Order()
        {
            var shippingMethods = new List<ShippingMethod>() { new ShippingMethod() { Id = 1, Cost = 0 } };
            var shoppingCartItems = new List<ShoppingCartItem>() { new ShoppingCartItem() { Quantity = 1, Currency = "USD", Product = new Product() } };
            var order = new Order()
            {
                ShoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD", FullPrice = 100 },
                ShippingAddress = new Address(),
                BillingAddress = new Address(),
                PaymentMethod = new PaymentMethod() { CardNumber = "1234" },
                ShippingMethod = shippingMethods.First()
            };
            var shippingMethodService = new MockShippingMethodService()
            {
                GetShippingMethodsAsyncDelegate = () => Task.FromResult<IEnumerable<ShippingMethod>>(shippingMethods)
            };
            var flyoutService = new MockFlyoutService();
            flyoutService.ShowFlyoutDelegate = (flyoutName, param, success) =>
            { 
                // Update CheckoutData information and call success
                            ((PaymentMethod)param).CardNumber = "5678";
                success.Invoke();
            };
            var orderRepository = new MockOrderRepository() { GetCurrentOrderAsyncDelegate = () => order };
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(order.ShoppingCart);

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), new MockOrderService(), orderRepository, shippingMethodService,
                                                          null, shoppingCartRepository,
                                                          new MockAccountService(), flyoutService, new MockResourceLoader(), null);

            target.OnNavigatedTo(null, NavigationMode.New, null);
            target.SelectedCheckoutData = target.CheckoutDataViewModels[2];
            target.EditCheckoutDataCommand.Execute().Wait();

            // Check if order information has changed
            Assert.IsTrue(order.PaymentMethod.CardNumber == "5678");
            Assert.IsTrue(((PaymentMethod)target.CheckoutDataViewModels[2].Context).CardNumber == "5678");
        }

        [TestMethod]
        public void AddCheckoutData_Calls_Proper_Flyout()
        {
            string requestedFlyoutName = string.Empty;
            var MockFlyoutService = new MockFlyoutService()
            {
                ShowFlyoutDelegate = (flyoutName, action, b) =>
                {
                    Assert.IsTrue(flyoutName == requestedFlyoutName);
                }
            };

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), null, null, null, null, null, null, MockFlyoutService, null, null);

            requestedFlyoutName = "ShippingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.ShippingAddress, null);
            target.AddCheckoutDataCommand.Execute().Wait();

            requestedFlyoutName = "BillingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.BillingAddress, null);
            target.AddCheckoutDataCommand.Execute().Wait();

            requestedFlyoutName = "PaymentMethod";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.PaymentMethod, null);
            target.AddCheckoutDataCommand.Execute().Wait();
        }
    }
}
