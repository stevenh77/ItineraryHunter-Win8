// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.ComponentModel;
using System.Globalization;
using Kona.Infrastructure;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace Kona.AWShopper.Views
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : VisualStateAwarePage
    {
        double itemGridViewScrollViewerHorizontalOffset;
        private ScrollViewer itemGridViewScrollViewer = null;

        public GroupDetailPage()
        {
            InitializeComponent();
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
            var adventureWorksApp = Application.Current as App;
            if (adventureWorksApp != null && (!adventureWorksApp.IsSuspending && viewModel != null))
            {
                viewModel.PropertyChanged -= viewModel_PropertyChanged;
            }
        }

        void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Items")
            {
                var listViewBase = GroupDetailssemanticZoom.ZoomedOutView as ListViewBase;
                if (listViewBase != null)
                    listViewBase.ItemsSource = ItemsViewSource.View.CollectionGroups;
            }
        }


        void itemGridView_Loaded(object sender, RoutedEventArgs e)
        {
            // Find the ScrollViewer inside the GridView
            itemGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemGridView);
            if (itemGridViewScrollViewer != null)
            {
                // Use a helper that lets you watch a dependency property on an object for changes
                // In this case, watching for the horizontal scroll bar to become visible
                DependencyPropertyChangedHelper helper = new DependencyPropertyChangedHelper(itemGridViewScrollViewer, "ComputedHorizontalScrollBarVisibility");
                // When the scrollbar becomes visible - update scroll location to last scroll location before we scrolled away
                helper.PropertyChanged += (s, e2) =>
                {
                    var isVisible = ((Visibility)e2.NewValue) == Windows.UI.Xaml.Visibility.Visible;
                    if (isVisible)
                        ScrollToSavedHorizontalOffset();
                };
            }
        }

        private void ScrollToSavedHorizontalOffset()
        {
            itemGridViewScrollViewer.ScrollToHorizontalOffset(itemGridViewScrollViewerHorizontalOffset);
        }

        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            base.SaveState(pageState);

            if (itemGridViewScrollViewer != null && pageState != null)
            {
                pageState["itemGridViewScrollViewerHorizontalOffset"] = itemGridViewScrollViewer.HorizontalOffset;
            }
        }

        protected override void LoadState(object navigationParameter, System.Collections.Generic.Dictionary<string, object> pageState)
        {
            base.LoadState(navigationParameter, pageState);

            if (pageState != null && pageState.ContainsKey("itemGridViewScrollViewerHorizontalOffset"))
            {
                itemGridViewScrollViewerHorizontalOffset = double.Parse(pageState["itemGridViewScrollViewerHorizontalOffset"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }

    }
}
