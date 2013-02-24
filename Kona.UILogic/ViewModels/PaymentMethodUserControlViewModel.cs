// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public class PaymentMethodUserControlViewModel : ViewModel, IPaymentMethodUserControlViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private PaymentMethod _paymentMethod;

        public PaymentMethodUserControlViewModel(ICheckoutDataRepository checkoutDataRepository)
        {
            _paymentMethod = new PaymentMethod();
            _checkoutDataRepository = checkoutDataRepository;
        }

        [RestorableState]
        public PaymentMethod PaymentMethod
        {
            get { return _paymentMethod; }
            set { SetProperty(ref _paymentMethod, value); }
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            if (viewState != null)
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewState);

                if (navigationMode == NavigationMode.Refresh)
                {
                    // Restore the errors collection manually
                    var errorsCollection = RetrieveEntityStateValue<IDictionary<string, ReadOnlyCollection<string>>>("errorsCollection", viewState);

                    if (errorsCollection != null)
                    {
                        _paymentMethod.SetAllErrors(errorsCollection);
                    }
                }
            }

            if (navigationMode == NavigationMode.New)
            {
                var defaultPaymentMethod = await _checkoutDataRepository.GetDefaultPaymentMethodAsync();
                if (defaultPaymentMethod != null)
                {
                    // Update the information and validate the values
                    PaymentMethod.CardNumber = defaultPaymentMethod.CardNumber;
                    PaymentMethod.CardVerificationCode = defaultPaymentMethod.CardVerificationCode;
                    PaymentMethod.CardholderName = defaultPaymentMethod.CardholderName;
                    PaymentMethod.ExpirationMonth = defaultPaymentMethod.ExpirationMonth;
                    PaymentMethod.ExpirationYear = defaultPaymentMethod.ExpirationYear;
                    PaymentMethod.Phone = defaultPaymentMethod.Phone;

                    ValidateForm();
                }
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            base.OnNavigatedFrom(viewState, suspending);

            // Store the errors collection manually
            if (viewState != null)
            {
                AddEntityStateValue("errorsCollection", _paymentMethod.GetAllErrors(), viewState);
            }
        }

        public void ProcessForm()
        {
            _checkoutDataRepository.SavePaymentMethodAsync(PaymentMethod);
        }

        public bool ValidateForm()
        {
            return _paymentMethod.ValidateProperties();
        }
    }
}
