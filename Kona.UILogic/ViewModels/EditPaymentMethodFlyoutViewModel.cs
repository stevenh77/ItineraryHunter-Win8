// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.Infrastructure.Flyouts;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;

namespace Kona.UILogic.ViewModels
{
    public class EditPaymentMethodFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IPaymentMethodUserControlViewModel _viewModel;

        public EditPaymentMethodFlyoutViewModel(IPaymentMethodUserControlViewModel paymentMethodUserControlViewModel, ICheckoutDataRepository checkoutDataRepository)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _viewModel = paymentMethodUserControlViewModel;
            SaveCommand = new DelegateCommand(SavePaymentInfo);
            GoBackCommand = new DelegateCommand(() => GoBack(), () => true);
        }

        public IPaymentMethodUserControlViewModel PaymentMethodUserControlViewModel { get { return _viewModel; } }
        public ICommand SaveCommand { get; set; }
        public Action CloseFlyout { get; set; }
        public Action GoBack { get; set; }
        public ICommand GoBackCommand { get; private set; }

        public void Open(object parameter, Action successAction)
        {
            var paymentInfoId = parameter as string;
            if (paymentInfoId == null) return;
            _viewModel.PaymentInfo = _checkoutDataRepository.RetrievePaymentInformation(paymentInfoId);
        }

        private void SavePaymentInfo()
        {
            if (PaymentMethodUserControlViewModel.ValidateForm())
            {
                _checkoutDataRepository.SavePaymentInfo(PaymentMethodUserControlViewModel.PaymentInfo);
                CloseFlyout();
                //TODO: Set this as the payment info to use
            }
        }
    }
}
