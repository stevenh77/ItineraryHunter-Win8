// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.Infrastructure.Interfaces;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using System.Net.Http;
using System.Globalization;
using Windows.UI.ViewManagement;

namespace Kona.UILogic.ViewModels
{
    public class CheckoutHubPageViewModel : ViewModel, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IAccountService _accountService;
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShippingAddressUserControlViewModel _shippingAddressViewModel;
        private readonly IBillingAddressUserControlViewModel _billingAddressViewModel;
        private readonly IPaymentMethodUserControlViewModel _paymentMethodViewModel;
        private readonly IFlyoutService _flyoutService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private bool _useSameAddressAsShipping;
        private bool _isShippingAddressInvalid;
        private bool _isBillingAddressInvalid;
        private bool _isPaymentMethodInvalid;

        public CheckoutHubPageViewModel(INavigationService navigationService, IAccountService accountService, IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository,
                                        IShippingAddressUserControlViewModel shippingAddressViewModel, IBillingAddressUserControlViewModel billingAddressViewModel, IPaymentMethodUserControlViewModel paymentMethodViewModel,
                                        IFlyoutService flyoutService, IResourceLoader resourceLoader, IAlertMessageService alertMessageService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _shippingAddressViewModel = shippingAddressViewModel;
            _billingAddressViewModel = billingAddressViewModel;
            _paymentMethodViewModel = paymentMethodViewModel;
            _flyoutService = flyoutService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;

            GoBackCommand = new DelegateCommand(navigationService.GoBack, navigationService.CanGoBack);
            GoNextCommand = new DelegateCommand(GoNext);
        }

        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoNextCommand { get; private set; }

        public IShippingAddressUserControlViewModel ShippingAddressViewModel { get { return _shippingAddressViewModel; } }
        public IBillingAddressUserControlViewModel BillingAddressViewModel { get { return _billingAddressViewModel; } }
        public IPaymentMethodUserControlViewModel PaymentMethodViewModel { get { return _paymentMethodViewModel; } }

        [RestorableState]
        public bool UseSameAddressAsShipping
        {
            get { return _useSameAddressAsShipping; }
            set
            {
                SetProperty(ref _useSameAddressAsShipping, value);
                if (_useSameAddressAsShipping)
                {
                    // Clean the Billing Address values & errors
                    BillingAddressViewModel.Address = new Address { Id = Guid.NewGuid().ToString() };
                }

                BillingAddressViewModel.IsEnabled = !value;
            }
        }

        [RestorableState]
        public bool IsShippingAddressInvalid
        {
            get { return _isShippingAddressInvalid; }
            set { SetProperty(ref _isShippingAddressInvalid, value); }
        }

        [RestorableState]
        public bool IsBillingAddressInvalid
        {
            get { return _isBillingAddressInvalid; }
            set { SetProperty(ref _isBillingAddressInvalid, value); }
        }

        [RestorableState]
        public bool IsPaymentMethodInvalid
        {
            get { return _isPaymentMethodInvalid; }
            set { SetProperty(ref _isPaymentMethodInvalid, value); }
        }

        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            ShippingAddressViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewState);
            BillingAddressViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewState);
            PaymentMethodViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewState);
            base.OnNavigatedTo(navigationParameter, navigationMode, viewState);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            ShippingAddressViewModel.OnNavigatedFrom(viewState, suspending);
            BillingAddressViewModel.OnNavigatedFrom(viewState, suspending);
            PaymentMethodViewModel.OnNavigatedFrom(viewState, suspending);
            base.OnNavigatedFrom(viewState, suspending);
        }

        private async void GoNext()
        {
            IsShippingAddressInvalid = ShippingAddressViewModel.ValidateForm() == false;
            IsBillingAddressInvalid = !UseSameAddressAsShipping && BillingAddressViewModel.ValidateForm() == false;
            IsPaymentMethodInvalid = PaymentMethodViewModel.ValidateForm() == false;

            if (IsShippingAddressInvalid || IsBillingAddressInvalid || IsPaymentMethodInvalid) return;

                if (await _accountService.GetSignedInUserAsync() == null)
                {
                    _flyoutService.ShowFlyout("SignIn", null, successAction: async () => await ProcessFormAsync());
                }
                else
                {
                    await ProcessFormAsync();
                }
            }

        private async Task ProcessFormAsync()
        {
            string errorMessage = string.Empty;
            var user = await _accountService.GetSignedInUserAsync();
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();

            // <snippet912>
            try
            {
                // If everything is OK, process the form information and navigate to the next page
                if (UseSameAddressAsShipping)
                {
                    BillingAddressViewModel.Address = ShippingAddressViewModel.Address;
                }

                ShippingAddressViewModel.ProcessForm();
                BillingAddressViewModel.ProcessForm();
                PaymentMethodViewModel.ProcessForm();

                // Create an order with the values entered in the form
                await _orderRepository.CreateBasicOrderAsync(user.UserName, shoppingCart, ShippingAddressViewModel.Address,
                                                                            BillingAddressViewModel.Address,
                                                                            PaymentMethodViewModel.PaymentMethod);

                _navigationService.Navigate("CheckoutSummary", null);
            }
            catch (ModelValidationException mvex)
            {
                DisplayOrderErrorMessages(mvex.ValidationResult);
            }
            // </snippet912>

            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorProcessingOrder"));
            }
        }

        private void DisplayOrderErrorMessages(ModelValidationResult validationResult)
        {
            var shippingAddressErrors = new Dictionary<string, ReadOnlyCollection<string>>();
            var billingAddressErrors = new Dictionary<string, ReadOnlyCollection<string>>();
            var paymentMethodErrors = new Dictionary<string, ReadOnlyCollection<string>>();

            // Property keys of the form. Format: order.{ShippingAddress/BillingAddress/PaymentMethod}.{Property}
            foreach (var propkey in validationResult.ModelState.Keys)
            {
                string orderPropAndEntityProp = propkey.Substring(propkey.IndexOf('.') + 1); // strip off order. prefix
                string orderProperty = orderPropAndEntityProp.Substring(0, orderPropAndEntityProp.IndexOf('.') + 1);
                string entityProperty = orderPropAndEntityProp.Substring(orderProperty.IndexOf('.') + 1);

                if (orderProperty.ToLower().Contains("shipping")) shippingAddressErrors.Add(entityProperty, new ReadOnlyCollection<string>(validationResult.ModelState[propkey]));
                if (orderProperty.ToLower().Contains("billing") && !UseSameAddressAsShipping) billingAddressErrors.Add(entityProperty, new ReadOnlyCollection<string>(validationResult.ModelState[propkey]));
                if (orderProperty.ToLower().Contains("payment")) paymentMethodErrors.Add(entityProperty, new ReadOnlyCollection<string>(validationResult.ModelState[propkey]));
            }

            if (shippingAddressErrors.Count > 0) _shippingAddressViewModel.Address.Errors.SetAllErrors(shippingAddressErrors);
            if (billingAddressErrors.Count > 0) _billingAddressViewModel.Address.Errors.SetAllErrors(billingAddressErrors);
            if (paymentMethodErrors.Count > 0) _paymentMethodViewModel.PaymentMethod.Errors.SetAllErrors(paymentMethodErrors);
        }
    }
}
