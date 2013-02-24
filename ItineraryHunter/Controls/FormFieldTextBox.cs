// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Kona.AWShopper.Controls
{
    public sealed class FormFieldTextBox : TextBox
    {
        public static readonly DependencyProperty WatermarkProperty = 
            DependencyProperty.Register("Watermark", typeof(string), typeof(FormFieldTextBox), new PropertyMetadata(string.Empty));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public FormFieldTextBox()
        {
            this.DefaultStyleKey = typeof(FormFieldTextBox);
            this.TextChanged += FormFieldTextBox_TextChanged;
            this.GotFocus += FormFieldTextBox_GotFocus;
            this.LostFocus += FormFieldTextBox_LostFocus;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Update watermark visibility
            this.UpdateWatermarkVisibility(string.IsNullOrEmpty(this.Text));
        }

        private void FormFieldTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.UpdateWatermarkVisibility(false);
        }

        void FormFieldTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.UpdateWatermarkVisibility(string.IsNullOrEmpty(this.Text));
        }

        private void FormFieldTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateWatermarkVisibility(string.IsNullOrEmpty(this.Text));
        }

        private void UpdateWatermarkVisibility(bool isVisible)
        {
            if (isVisible)
            {
                VisualStateManager.GoToState(this, "WatermarkVisible", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "WatermarkCollapsed", false);
            }
        }
    }
}
