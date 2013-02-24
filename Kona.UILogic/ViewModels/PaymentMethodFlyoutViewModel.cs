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
    public class PaymentMethodFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly IPaymentMethodUserControlViewModel _paymentMethodViewModel;
        private readonly IResourceLoader _resourceLoader;
        private string _headerLabel;
        private Action _successAction;

        public PaymentMethodFlyoutViewModel(IPaymentMethodUserControlViewModel paymentMethodViewModel, IResourceLoader resourceLoader)
        {
            _paymentMethodViewModel = paymentMethodViewModel;
            _resourceLoader = resourceLoader;
            
            SaveCommand = new DelegateCommand(SavePaymentMethod);
            GoBackCommand = new DelegateCommand(() => GoBack());
        }

        public IPaymentMethodUserControlViewModel PaymentMethodViewModel
        { 
            get { return _paymentMethodViewModel; } 
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

        public void Open(object parameter, Action successAction)
        {
            _successAction = successAction;

            var paymentMethod = parameter as PaymentMethod;

            if (paymentMethod != null)
            {
                PaymentMethodViewModel.PaymentMethod = paymentMethod;
                HeaderLabel = _resourceLoader.GetString("EditPaymentMethodTitle");
            }
            else
            {
                HeaderLabel = _resourceLoader.GetString("AddPaymentMethodTitle");
            }
        }

        private void SavePaymentMethod()
        {
            if (PaymentMethodViewModel.ValidateForm())
            {
                PaymentMethodViewModel.ProcessForm();
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