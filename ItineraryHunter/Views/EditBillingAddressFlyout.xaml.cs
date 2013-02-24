// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure.Flyouts;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Kona.AWShopper.Views
{
    public sealed partial class EditBillingAddressFlyout : FlyoutView
    {
        public EditBillingAddressFlyout(string commandId, string commandTitle)
            : base(commandId, commandTitle, StandardFlyoutSize.Narrow)
        {
            this.InitializeComponent();

            var viewModel = this.DataContext as IFlyoutViewModel;
            if (viewModel != null)
            {
                viewModel.CloseFlyout = this.Close;
                viewModel.GoBack = this.GoBack;
            }
        }
    }
}
