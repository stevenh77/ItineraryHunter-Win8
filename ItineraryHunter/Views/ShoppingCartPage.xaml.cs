// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Kona.Infrastructure.Interfaces;
using Windows.UI.Xaml;

namespace Kona.AWShopper.Views
{
    public sealed partial class ShoppingCartPage : VisualStateAwarePage
    {
        public ShoppingCartPage()
        {
            this.InitializeComponent();
            EditQuantityPopup.Opened += EditQuantityPopup_Opened;
        }

        private void EditQuantityPopup_Opened(object sender, object e)
        {
            int margin = 10;
            int appbarHeight = 90;
            EditQuantityPopup.HorizontalOffset = margin;
            EditQuantityPopup.VerticalOffset = Window.Current.CoreWindow.Bounds.Bottom - appbarHeight - PopupPanel.ActualHeight - margin;
        }
    }
}
