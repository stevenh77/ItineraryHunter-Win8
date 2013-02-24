// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.ComponentModel;
using System.Globalization;
using Kona.Infrastructure;
using Windows.UI.Xaml.Controls;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace Kona.AWShopper.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : VisualStateAwarePage
    {
        private double virtualizingStackPanelHorizontalOffset;

        public HubPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = this.DataContext as INotifyPropertyChanged;
            if (viewModel != null)
            {
                viewModel.PropertyChanged += viewModel_PropertyChanged;
            }
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var viewModel = this.DataContext as INotifyPropertyChanged;
            var adventureWorksApp = App.Current as App;
            if (!adventureWorksApp.IsSuspending && viewModel != null)
            {
                viewModel.PropertyChanged -= viewModel_PropertyChanged;
            }
        }

        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RootCategories")
            {
                (semanticZoom.ZoomedOutView as ListViewBase).ItemsSource = groupedItemsViewSource.View.CollectionGroups;
            }
        }

        private void virtualizingStackPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var virtualizingStackPanel = (VirtualizingStackPanel)sender;
            virtualizingStackPanel.SetHorizontalOffset(virtualizingStackPanelHorizontalOffset);
        }


        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            base.SaveState(pageState);
            var virtualizingStackPanel = VisualTreeUtilities.GetVisualChild<VirtualizingStackPanel>(itemGridView);
            if (virtualizingStackPanel != null && pageState != null)
            {
                pageState["virtualizingStackPanelHorizontalOffset"] = virtualizingStackPanel.HorizontalOffset;
            }
        }

        protected override void LoadState(object navigationParameter, System.Collections.Generic.Dictionary<string, object> pageState)
        {
            base.LoadState(navigationParameter, pageState);
            if (pageState != null && pageState.ContainsKey("virtualizingStackPanelHorizontalOffset"))
            {
                virtualizingStackPanelHorizontalOffset = double.Parse(pageState["virtualizingStackPanelHorizontalOffset"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }

        }
    }
}
