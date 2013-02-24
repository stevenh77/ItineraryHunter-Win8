// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class CheckoutHubPageViewModelFixture
    {
        [TestMethod]
        public async Task ExecuteGoNextCommand_Validates3ViewModels()
        {
            bool shippingValidationExecuted = false;
            bool billingValidationExecuted = false;
            bool paymentValidationExecuted = false;
            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => { shippingValidationExecuted = true; return false; }
                };
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => { billingValidationExecuted = true; return false; }
                };
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel()
                {
                    ValidateFormDelegate = () => { paymentValidationExecuted = true; return false; }
                };

            var target = new CheckoutHubPageViewModel(new MockNavigationService(), null, null, new MockShoppingCartRepository(),
                                                        shippingAddressPageViewModel, billingAddressPageViewModel, paymentMethodPageViewModel, null, null, null);
            await target.GoNextCommand.Execute();

            Assert.IsTrue(shippingValidationExecuted);
            Assert.IsTrue(billingValidationExecuted);
            Assert.IsTrue(paymentValidationExecuted);
        }

        [TestMethod]
        public async Task ExecuteGoNextCommand_ProcessesFormsAndNavigates_IfViewModelsAreValid()
        {
            bool shippingInfoProcessed = false;
            bool billingInfoProcessed = false;
            bool paymentInfoProcessed = false;
            bool navigated = false;
            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormDelegate = () => shippingInfoProcessed = true
                };
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormDelegate = () => billingInfoProcessed = true
                };
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormDelegate = () => paymentInfoProcessed = true
                };
            var accountService = new MockAccountService()
                {
                    GetSignedInUserAsyncDelegate = () => Task.FromResult(new UserInfo())
                };
            var orderRepository = new MockOrderRepository()
                {
                    CreateBasicOrderAsyncDelegate = (a, b, c, d, e) => Task.FromResult(new Order() { Id = 1 })
                };
            var shoppingCartRepository = new MockShoppingCartRepository()
                {
                    GetShoppingCartAsyncDelegate = () => Task.FromResult(new ShoppingCart(null))
                };
            var navigationService = new MockNavigationService()
                {
                    NavigateDelegate = (a, b) => navigated = true
                };

            var target = new CheckoutHubPageViewModel(navigationService, accountService, orderRepository, shoppingCartRepository,
                                            shippingAddressPageViewModel, billingAddressPageViewModel, paymentMethodPageViewModel, null, null, null);
            await target.GoNextCommand.Execute();

            Assert.IsTrue(shippingInfoProcessed);
            Assert.IsTrue(billingInfoProcessed);
            Assert.IsTrue(paymentInfoProcessed);
            Assert.IsTrue(navigated);
        }

        [TestMethod]
        public async Task ExecuteGoNextCommand_DoNothing_IfViewModelsAreInvalid()
        {
            bool formProcessStarted = false;
            var accountService = new MockAccountService()
            {
                GetSignedInUserAsyncDelegate = () =>
                    {
                        // The process starts with a call to retrieve the logged user
                        formProcessStarted = true;
                        return Task.FromResult(new UserInfo());
                    }
            };
            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel();
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel();
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel();
            var target = new CheckoutHubPageViewModel(new MockNavigationService(), accountService, null, null,
                                                       shippingAddressPageViewModel, billingAddressPageViewModel, paymentMethodPageViewModel, null, null, null);

            // ShippingAddress invalid only
            shippingAddressPageViewModel.ValidateFormDelegate = () => false;
            billingAddressPageViewModel.ValidateFormDelegate = () => true;
            paymentMethodPageViewModel.ValidateFormDelegate = () => true;
            await target.GoNextCommand.Execute();

            Assert.IsFalse(formProcessStarted);

            // BillingAddress invalid only
            shippingAddressPageViewModel.ValidateFormDelegate = () => true;
            billingAddressPageViewModel.ValidateFormDelegate = () => false;
            paymentMethodPageViewModel.ValidateFormDelegate = () => true;

            Assert.IsFalse(formProcessStarted);

            // PaymentMethod invalid only
            shippingAddressPageViewModel.ValidateFormDelegate = () => true;
            billingAddressPageViewModel.ValidateFormDelegate = () => true;
            paymentMethodPageViewModel.ValidateFormDelegate = () => false;

            Assert.IsFalse(formProcessStarted);
        }

        [TestMethod]
        public async Task ExecuteGoNextCommand_DisplaysSignInFlyout_IfViewModelsAreValidAndUserNotLoggedIn()
        {
            bool signInFlyoutDisplayed = false;
            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true
                };
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true
                };
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel()
                {
                    ValidateFormDelegate = () => true
                };
            var accountService = new MockAccountService()
                {
                    GetSignedInUserAsyncDelegate = () => Task.FromResult<UserInfo>(null)
                };
            var flyoutService = new MockFlyoutService()
                {
                    ShowFlyoutDelegate = (a, b, c) => signInFlyoutDisplayed = true
                };

            var target = new CheckoutHubPageViewModel(new MockNavigationService(), accountService, null, null, shippingAddressPageViewModel,
                                                      billingAddressPageViewModel, paymentMethodPageViewModel, flyoutService, null, null);
            await target.GoNextCommand.Execute();

            Assert.IsTrue(signInFlyoutDisplayed);
        }

        [TestMethod]
        public async Task SettingUseShippingAddressToTrue_CopiesValuesFromShippingAddressToBilling()
        {
            var mockAddress = new Address()
                {
                    FirstName = "TestFirstName",
                    MiddleInitial = "TestMiddleInitial",
                    LastName = "TestLastName",
                    StreetAddress = "TestStreetAddress",
                    OptionalAddress = "TestOptionalAddress",
                    City = "TestCity",
                    State = "TestState",
                    ZipCode = "123456",
                    Phone = "123456"
                };
            var compareAddressesFunc = new Func<Address, Address, bool>((Address a1, Address a2) =>
                {
                    return a1.FirstName == a2.FirstName && a1.MiddleInitial == a2.MiddleInitial && a1.LastName == a2.LastName
                           && a1.StreetAddress == a2.StreetAddress && a1.OptionalAddress == a2.OptionalAddress && a1.City == a2.City
                           && a1.State == a2.State && a1.ZipCode == a2.ZipCode && a1.Phone == a2.Phone;
                });

            var shippingAddressPageViewModel = new MockShippingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormDelegate = () => Task.Delay(0),
                    Address = mockAddress
                };
            var billingAddressPageViewModel = new MockBillingAddressPageViewModel()
                {
                    ValidateFormDelegate = () => true
                };
            billingAddressPageViewModel.ProcessFormDelegate = () =>
                {
                    // The Address have to be updated before the form is processed
                    Assert.IsTrue(compareAddressesFunc(shippingAddressPageViewModel.Address, billingAddressPageViewModel.Address));
                };
            var paymentMethodPageViewModel = new MockPaymentMethodPageViewModel()
                {
                    ValidateFormDelegate = () => true,
                    ProcessFormDelegate = () => Task.Delay(0),
                };
            var accountService = new MockAccountService()
                {
                    GetSignedInUserAsyncDelegate = () => Task.FromResult(new UserInfo())
                };
            var orderRepository = new MockOrderRepository()
                {
                    CreateBasicOrderAsyncDelegate = (userId, shoppingCart, shippingAddress, billingAddress, paymentMethod) =>
                        {
                            // The Address information stored in the order must be the same
                            Assert.IsTrue(compareAddressesFunc(shippingAddress, billingAddress));
                            return Task.FromResult<Order>(new Order());
                        }
                };
            var shippingMethodService = new MockShippingMethodService()
                {
                    GetBasicShippingMethodAsyncDelegate = () => Task.FromResult(new ShippingMethod())
                };
            var shoppingCartRepository = new MockShoppingCartRepository()
                {
                    GetShoppingCartAsyncDelegate = () => Task.FromResult(new ShoppingCart(null))
                };
            var navigationService = new MockNavigationService()
                {
                    NavigateDelegate = (a, b) => true
                };

            var target = new CheckoutHubPageViewModel(navigationService, accountService, orderRepository, shoppingCartRepository,
                                            shippingAddressPageViewModel, billingAddressPageViewModel, paymentMethodPageViewModel, null, null, null);
            target.UseSameAddressAsShipping = true;

            await target.GoNextCommand.Execute();
        }
    }
}
