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
    public class BillingAddressFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly IBillingAddressUserControlViewModel _billingAddressViewModel;
        private readonly IResourceLoader _resourceLoader;
        private string _headerLabel;
        private Action _successAction;

        public BillingAddressFlyoutViewModel(IBillingAddressUserControlViewModel billingAddressViewModel, IResourceLoader resourceLoader)
        {
            _billingAddressViewModel = billingAddressViewModel;
            _resourceLoader = resourceLoader;

            SaveCommand = new DelegateCommand(SaveBillingAddress);
            GoBackCommand = new DelegateCommand(() => GoBack());
        }

        public IBillingAddressUserControlViewModel BillingAddressViewModel
        {
            get { return _billingAddressViewModel; } 
        }

        public string HeaderLabel
        {
            get { return _headerLabel; }
            set { SetProperty(ref _headerLabel, value); }
        }

        public Action CloseFlyout { get; set; }

        public Action GoBack { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand GoBackCommand { get; private set; }

        public async void Open(object parameter, Action successAction)
        {
            _successAction = successAction;
            await BillingAddressViewModel.PopulateStatesAsync();

            var billingAddress = parameter as Address;

            if (billingAddress != null)
            {
                BillingAddressViewModel.Address = billingAddress;
                HeaderLabel = _resourceLoader.GetString("EditBillingAddressTitle");
            }
            else
            {
                HeaderLabel = _resourceLoader.GetString("AddBillingAddressTitle");
            }
        }

        private void SaveBillingAddress()
        {
            if (BillingAddressViewModel.ValidateForm())
            {
                BillingAddressViewModel.ProcessForm();
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
