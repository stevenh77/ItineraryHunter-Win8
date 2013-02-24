// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.Infrastructure.Flyouts;
using Kona.Infrastructure.Interfaces;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Kona.Infrastructure
{
    public class FlyoutService : IFlyoutService
    {
        private bool _isUnsnapping;
        private string _flyoutId;
        private object _parameter;
        private Action _successAction;

        public FlyoutService()
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (_isUnsnapping)
            {
                ShowFlyout(_flyoutId, _parameter, _successAction);
                _isUnsnapping = false;
            }
        }

        public Func<string, FlyoutView> FlyoutResolver { get; set; }

        public void ShowFlyout(string flyoutId, object parameter, Action successAction)
        {
            if (FlyoutResolver == null) return;

            if (ApplicationView.Value == ApplicationViewState.Snapped)
            {
                _isUnsnapping = true;
                
                // Save ShowFlyout parameters so that they can be called in Current_SizeChanged handler
                _flyoutId = flyoutId;
                _parameter = parameter;
                _successAction = successAction;
                ApplicationView.TryUnsnap();
            }
            else
            {
                var flyout = FlyoutResolver(flyoutId);

                if (flyout != null)
                {
                    flyout.Open(parameter, successAction);
                }
            }
        }

        public void ShowFlyout(string flyoutId)
        {
            ShowFlyout(flyoutId, null, null);
        }
    }
}
