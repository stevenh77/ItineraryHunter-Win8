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
using Kona.UILogic.Repositories;

namespace Kona.UILogic.ViewModels
{
    public class EditShippingAddressFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IShippingAddressUserControlViewModel _viewModel;

        public EditShippingAddressFlyoutViewModel(IShippingAddressUserControlViewModel shippingAddressUserControlViewModel, ICheckoutDataRepository checkoutDataRepository)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _viewModel = shippingAddressUserControlViewModel;
            SaveCommand = new DelegateCommand(SaveShippingAddress);
            GoBackCommand = new DelegateCommand(() => GoBack(), () => true);
        }

        public IShippingAddressUserControlViewModel ShippingAddressUserControlViewModel { get { return _viewModel; } }
        public ICommand SaveCommand { get; set; }
        public Action CloseFlyout { get; set; }
        public Action GoBack { get; set; }
        public ICommand GoBackCommand { get; private set; }

        public async void Open(object parameter, Action successAction)
        {
            var shippingAddressId = parameter as string;
            if (shippingAddressId == null) return;
            var shippingAddress = _checkoutDataRepository.RetrieveShippingAddress(shippingAddressId);
            await _viewModel.PopulateStatesAsync();
            _viewModel.Address = shippingAddress;
        }

        private void SaveShippingAddress()
        {
            if (ShippingAddressUserControlViewModel.ValidateForm())
            {
                _checkoutDataRepository.SaveShippingAddress(ShippingAddressUserControlViewModel.Address);
                CloseFlyout();
                //TODO: Set this as the payment info to use
            }
        }
    }
}
