// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure.Flyouts;
using Kona.Infrastructure.Interfaces;
using Windows.UI.Xaml;

namespace Kona.AWShopper.Views
{
    public sealed partial class ChangeDefaultsFlyout : FlyoutView
    {
        public ChangeDefaultsFlyout(ISearchPaneService searchPaneService)
            : base(StandardFlyoutSize.Narrow, searchPaneService)
        {
            InitializeComponent();
        }

        private void ExpandShippingAddressButton_Click(object sender, RoutedEventArgs e)
        {
            // Display the proper popup, hide the rest
            ShippingAddressesPopup.IsOpen = true;
            BillingAddressesPopup.IsOpen = false;
            PaymentMethodsPopup.IsOpen = false;
        }

        private void ExpandBillingAddressButton_Click(object sender, RoutedEventArgs e)
        {
            // Display the proper popup, hide the rest
            ShippingAddressesPopup.IsOpen = false;
            BillingAddressesPopup.IsOpen = true;
            PaymentMethodsPopup.IsOpen = false;
        }

        private void ExpandPaymentMethodButton_Click(object sender, RoutedEventArgs e)
        {
            // Display the proper popup, hide the rest
            ShippingAddressesPopup.IsOpen = false;
            BillingAddressesPopup.IsOpen = false;
            PaymentMethodsPopup.IsOpen = true;
        }
    }
}
