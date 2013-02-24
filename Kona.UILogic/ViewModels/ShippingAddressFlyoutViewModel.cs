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
    public class ShippingAddressFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly IShippingAddressUserControlViewModel _shippingAddressViewModel;
        private readonly IResourceLoader _resourceLoader;
        private string _headerLabel;
        private Action _successAction;

        public ShippingAddressFlyoutViewModel(IShippingAddressUserControlViewModel shippingAddressViewModel, IResourceLoader resourceLoader)
        {
            _shippingAddressViewModel = shippingAddressViewModel;
            _resourceLoader = resourceLoader;

            SaveCommand = new DelegateCommand(SavedShippingAddress);
            GoBackCommand = new DelegateCommand(() => GoBack());
        }

        public IShippingAddressUserControlViewModel ShippingAddressViewModel
        { 
            get { return _shippingAddressViewModel; } 
        }

        public string HeaderLabel
        {
            get { return _headerLabel; }
            set { SetProperty(ref _headerLabel, value); }
        }

        public ICommand SaveCommand { get; set; }
        
        public Action CloseFlyout { get; set; }
        
        public Action GoBack { get; set; }
        
        public ICommand GoBackCommand { get; private set; }

        public async void Open(object parameter, Action successAction)
        {
            _successAction = successAction;
            await ShippingAddressViewModel.PopulateStatesAsync();

            var shippingAddress = parameter as Address;

            if (shippingAddress != null)
            {
                ShippingAddressViewModel.Address = shippingAddress;
                HeaderLabel = _resourceLoader.GetString("EditShippingAddressTitle");
            }
            else
            {
                HeaderLabel = _resourceLoader.GetString("AddShippingAddressTitle");
            }
        }

        private void SavedShippingAddress()
        {
            if (ShippingAddressViewModel.ValidateForm())
            {
                ShippingAddressViewModel.ProcessForm();
                CloseFlyout();

                if (_successAction != null)
                {
                    _successAction();
                    _successAction = null;
                }
            }
        }
    }
}