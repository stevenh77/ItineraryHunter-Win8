// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.Infrastructure.Flyouts;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;

namespace Kona.UILogic.ViewModels
{
    public class EditBillingAddressFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IBillingAddressUserControlViewModel _viewModel;

        public EditBillingAddressFlyoutViewModel(IBillingAddressUserControlViewModel billingAddressUserControlViewModel, ICheckoutDataRepository checkoutDataRepository)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _viewModel = billingAddressUserControlViewModel;
            SaveCommand = new DelegateCommand(SaveBillingAddress);
            GoBackCommand = new DelegateCommand(() => GoBack(), () => true);
        }

        public IBillingAddressUserControlViewModel BillingAddressUserControlViewModel { get { return _viewModel; } }
        public ICommand SaveCommand { get; set; }
        public Action CloseFlyout { get; set; }
        public Action GoBack { get; set; }
        public ICommand GoBackCommand { get; private set; }

        public async void Open(object parameter, Action successAction)
        {
            var billingAddressId = parameter as string;
            if (billingAddressId == null) return;
            _viewModel.Address = _checkoutDataRepository.RetrieveBillingAddress(billingAddressId);

            await _viewModel.PopulateStatesAsync();
        }

        private void SaveBillingAddress()
        {
            if (BillingAddressUserControlViewModel.ValidateForm())
            {
                _checkoutDataRepository.SaveShippingAddress(_viewModel.Address);
                CloseFlyout();
                //TODO: Set this as the payment info to use
            }
        }
    }
}
