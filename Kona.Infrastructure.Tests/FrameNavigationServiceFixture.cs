// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kona.Infrastructure.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Kona.Infrastructure.Tests
{
    [TestClass]
    public class FrameNavigationServiceFixture
    {
        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }

        [TestMethod]
        public async Task Navigate_To_Valid_Page()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var frameSessionState = new MockFrameSessionState();
                frameSessionState.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();
                var restorableStateService = new MockSuspensionManagerState();
                var navigationService = new FrameNavigationService(frame, frameSessionState, (pageToken) => typeof(MockPage), restorableStateService);

                bool result = navigationService.Navigate("Mock", 1);

                Assert.IsTrue(result);
                Assert.IsNotNull(frame.Content);
                Assert.IsInstanceOfType(frame.Content, typeof(MockPage));
                Assert.AreEqual(1, ((MockPage)frame.Content).PageParameter);
            });
        }

        [TestMethod]
        public async Task Navigate_To_Invalid_Page()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var frameSessionState = new MockFrameSessionState();
                frameSessionState.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();

                var navigationService = new FrameNavigationService(frame, frameSessionState, (pageToken) => typeof(string), null);

                bool result = navigationService.Navigate("Mock", 1);

                Assert.IsFalse(result);
                Assert.IsNull(frame.Content);
            });
        }

        [TestMethod]
        public async Task NavigateToCurrentViewModel_Calls_VieModel_OnNavigatedTo()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());

                bool viewModelNavigatedToCalled = false;
                var viewModel = new MockPageViewModel();
                viewModel.OnNavigatedFromCommand = (a, b) => Assert.IsTrue(true);
                viewModel.OnNavigatedToCommand = (parameter, navigationMode, frameState) =>
                {
                    Assert.AreEqual(NavigationMode.New, navigationMode);
                    Assert.AreEqual(1, frameState["someValue"]);
                    viewModelNavigatedToCalled = true;
                };

                // Set up the viewModel to the Page we navigated
                frame.Navigated += (sender, e) =>
                {
                    var view = frame.Content as FrameworkElement;
                    view.DataContext = viewModel;
                };

                var frameSessionState = new MockFrameSessionState();
                frameSessionState.GetSessionStateForFrameDelegate = (currentFrame) =>
                {
                    var toReturn = new Dictionary<string, object>();
                    toReturn.Add("someValue", 1);
                    return toReturn;
                };
                var restorableStateService = new MockSuspensionManagerState();
                var navigationService = new FrameNavigationService(frame, frameSessionState, (pageToken) => typeof(MockPage), restorableStateService);

                navigationService.Navigate("Mock", 1);

                Assert.IsTrue(viewModelNavigatedToCalled);
            });
        }

        [TestMethod]
        public async Task NavigateFromCurrentViewModel_Calls_VieModel_OnNavigatedFrom()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                

                bool viewModelNavigatedFromCalled = false;
                var viewModel = new MockPageViewModel();
                viewModel.OnNavigatedFromCommand = (frameState, suspending) =>
                {
                    Assert.IsFalse(suspending);
                    Assert.AreEqual(1, frameState["someValue"]);
                    viewModelNavigatedFromCalled = true;
                };

                var frameSessionState = new MockFrameSessionState();
                frameSessionState.GetSessionStateForFrameDelegate = (currentFrame) =>
                {
                    var toReturn = new Dictionary<string, object>();
                    toReturn.Add("someValue", 1);
                    return toReturn;
                };

                var navigationService = new FrameNavigationService(frame, frameSessionState, (pageToken) => typeof(MockPage), null);

                // Set up the frame's content with a Page
                var view = new MockPage();
                view.DataContext = viewModel;
                frame.Content = view;

                // Navigate to fire the NavigatedToCurrentViewModel
                navigationService.Navigate("Mock", 1);

                Assert.IsTrue(viewModelNavigatedFromCalled);
            });
        }

        [TestMethod]
        public async Task Suspending_Calls_VieModel_OnNavigatedFrom()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var frameSessionState = new MockFrameSessionState();
                frameSessionState.GetSessionStateForFrameDelegate = (currentFrame) =>
                {
                    var toReturn = new Dictionary<string, object>();
                    toReturn.Add("someValue", 1);
                    return toReturn;
                };
                var restorableStateService = new MockSuspensionManagerState();
                var navigationService = new FrameNavigationService(frame, frameSessionState, (pageToken) => typeof(MockPage), restorableStateService);

                navigationService.Navigate("Mock", 1);

                var viewModel = new MockPageViewModel();
                viewModel.OnNavigatedFromCommand = (frameState, suspending) =>
                {
                    Assert.IsTrue(suspending);
                    Assert.AreEqual(1, frameState["someValue"]);
                };

                var page = (MockPage)frame.Content;
                page.DataContext = viewModel;

                navigationService.Suspending();
            });
        }

        [TestMethod]
        public async Task Resuming_Calls_ViewModel_OnNavigatedTo()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var frameSessionState = new MockFrameSessionState();
                frameSessionState.GetSessionStateForFrameDelegate = (currentFrame) =>
                {
                    var toReturn = new Dictionary<string, object>();
                    toReturn.Add("someValue", 1);
                    return toReturn;
                };
                var restorableStateService = new MockSuspensionManagerState();
                var navigationService = new FrameNavigationService(frame, frameSessionState, (pageToken) => typeof(MockPage), restorableStateService);

                navigationService.Navigate("Mock", 1);

                var viewModel = new MockPageViewModel();
                viewModel.OnNavigatedToCommand = (navigationParameter, navigationMode, frameState) =>
                {
                    Assert.AreEqual(NavigationMode.Refresh, navigationMode);
                    Assert.AreEqual(1, frameState["someValue"]);
                };

                var page = (MockPage)frame.Content;
                page.DataContext = viewModel;

                navigationService.RestoreSavedNavigation();
            });
        }

        [TestMethod]
        public async Task GoBack_When_CanGoBack()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var frameSessionState = new MockFrameSessionState();
                frameSessionState.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();
                var suspensionManagerState = new MockSuspensionManagerState();
                var navigationService = new FrameNavigationService(frame, frameSessionState, (pageToken) => typeof(MockPage), suspensionManagerState);

                bool resultFirstNavigation = navigationService.Navigate("MockPage", 1);

                Assert.IsInstanceOfType(frame.Content, typeof(MockPage));
                Assert.AreEqual(1, ((MockPage)frame.Content).PageParameter);
                Assert.IsFalse(navigationService.CanGoBack());

                bool resultSecondNavigation = navigationService.Navigate("Mock", 2);

                Assert.IsInstanceOfType(frame.Content, typeof(MockPage));
                Assert.AreEqual(2, ((MockPage)frame.Content).PageParameter);
                Assert.IsTrue(navigationService.CanGoBack());

                navigationService.GoBack();

                Assert.IsInstanceOfType(frame.Content, typeof(MockPage));
                Assert.AreEqual(1, ((MockPage)frame.Content).PageParameter);
                Assert.IsFalse(navigationService.CanGoBack());
            });
        }
    }
}
