// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Kona.Infrastructure.Interfaces;

namespace Kona.Infrastructure
{
    public class FrameNavigationService : INavigationService
    {
        private const string LastNavigationParameterKey = "LastNavigationParameter";
        private readonly IFrameFacade _frame;
        private readonly IFrameSessionState _frameSessionState;
        private readonly Func<string, Type> _navigationResolver;
        private readonly ISuspensionManagerState _suspensionManagerState;

        public FrameNavigationService(IFrameFacade frame, IFrameSessionState frameSessionState, Func<string, Type> navigationResolver, ISuspensionManagerState suspensionManagerSessionState)
        {
            _frame = frame;
            _frameSessionState = frameSessionState;
            _navigationResolver = navigationResolver;
            _suspensionManagerState = suspensionManagerSessionState;

            _frame.Navigating += frame_Navigating;
            _frame.Navigated += frame_Navigated;
        }

        private void frame_Navigating(object sender, EventArgs e)
        {
            NavigateFromCurrentViewModel(false);
        }

        private void frame_Navigated(object sender, MvvmNavigatedEventArgs e)
        {
            _suspensionManagerState.SessionState[LastNavigationParameterKey] = e.Parameter;
            NavigateToCurrentViewModel(e.NavigationMode, e.Parameter);
        }

        public void Suspending()
        {
            NavigateFromCurrentViewModel(true);
        }

        public void RestoreSavedNavigation()
        {
            var navigationParameter = _suspensionManagerState.SessionState[LastNavigationParameterKey];
            NavigateToCurrentViewModel(NavigationMode.Refresh, navigationParameter);
        }

        // <snippet705>
        private void NavigateToCurrentViewModel(NavigationMode navigationMode, object parameter)
        {
            var newView = _frame.Content as FrameworkElement;
            if (newView == null) return;
            var frameState = _frameSessionState.GetSessionStateForFrame(_frame);
            var newViewModel = newView.DataContext as INavigationAware;
            if (newViewModel != null)
                newViewModel.OnNavigatedTo(parameter, navigationMode, frameState);
        }
        // </snippet705>

        // <snippet702>
        private void NavigateFromCurrentViewModel(bool suspending)
        {
            var departingView = _frame.Content as FrameworkElement;
            if (departingView == null) return;
            var frameState = _frameSessionState.GetSessionStateForFrame(_frame);
            var departingViewModel = departingView.DataContext as INavigationAware;
            if (departingViewModel != null)
                departingViewModel.OnNavigatedFrom(frameState, suspending);
        }
        // </snippet702>

        // <snippet413>
        public bool Navigate(string pageToken, object parameter)
        {
            Type pageType = _navigationResolver(pageToken);
            return _frame.Navigate(pageType, parameter);
        }
        // </snippet413>

        public void GoBack()
        {
            _frame.GoBack();
        }

        public bool CanGoBack()
        {
            return _frame.CanGoBack;
        }

        public void ClearHistory()
        {
            _frame.SetNavigationState("1,0");
        }
    }
}
