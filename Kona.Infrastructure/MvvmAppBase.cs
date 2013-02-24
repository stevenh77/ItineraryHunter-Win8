// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Kona.Infrastructure.Flyouts;
using Kona.Infrastructure.Interfaces;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Kona.Infrastructure
{
    /// <summary>
    /// Provides Kona application-specific behavior to supplement the default Application class.
    /// </summary>
    public abstract class MvvmAppBase : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public MvvmAppBase()
        {
            // <snippet700>
            this.Suspending += OnSuspending;
            // </snippet700>
        }

        public INavigationService NavigationService { get; set; }
        public ISuspensionManagerState SuspensionManagerState { get; set; }
        public IFlyoutService FlyoutService { get; set; }
        public SettingsCharmService SettingsCharmService { get; set; }
        public bool IsSuspending { get; private set; }

        public abstract void OnLaunchApplication(LaunchActivatedEventArgs args);
        public virtual void OnSearchApplication(SearchEventArgs args) { }

        public virtual Type GetPageType(string pageToken)
        {
            var assemblyQualifiedAppType = this.GetType().GetTypeInfo().AssemblyQualifiedName;

            var pageNameWithParameter = assemblyQualifiedAppType.Replace(this.GetType().FullName, this.GetType().Namespace + ".Views.{0}Page");

            var viewFullName = string.Format(CultureInfo.InvariantCulture, pageNameWithParameter, pageToken);
            var viewType = Type.GetType(viewFullName);

            return viewType;
        }

        public virtual void OnRegisterKnownTypesforSerialization() { }
        public virtual void OnInitialize(IActivatedEventArgs args) { }
        public virtual FlyoutView CreateFlyoutView(string flyoutName)
        {
            var assemblyQualifiedAppType = this.GetType().GetTypeInfo().AssemblyQualifiedName;

            var flyoutNameWithParameter = assemblyQualifiedAppType.Replace(this.GetType().FullName, this.GetType().Namespace + ".Views.{0}Flyout");

            var flyoutFullName = string.Format(CultureInfo.InvariantCulture, flyoutNameWithParameter, flyoutName);
            var flyoutType = Type.GetType(flyoutFullName);
            if (flyoutType == null)
            {
                throw new InvalidOperationException("Could not find associated Flyout in the Views folder.");
            }

            var flyoutInstance = Resolve(flyoutType);
            return flyoutInstance as FlyoutView;
        }

        public virtual IList<SettingsCharmItem> GetSettingsCharmItems()
        {
            return null;
        }

        public virtual object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        // <snippet704>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            var rootFrame = await InitializeFrameAsync(args);

            // <snippet404>
            if (rootFrame != null && rootFrame.Content == null)
            {
                OnLaunchApplication(args);
            }
            // </snippet404>

            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            if (AppManifestHelper.IsSearchDeclared())
            {
                // Register the Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
                // event in OnWindowCreated to speed up searches once the application is already running
                SearchPane.GetForCurrentView().QuerySubmitted += OnQuerySubmitted;
            }
        }

        protected async override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            // If the Window isn't already using Frame navigation, insert our own Frame
            var rootFrame = await InitializeFrameAsync(args);

            if (rootFrame != null)
            {
                var newArgs = new SearchEventArgs(args);
                OnSearchApplication(newArgs);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async Task<Frame> InitializeFrameAsync(IActivatedEventArgs args)
        {
            var rootFrame = Window.Current.Content as Frame;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                var frameFacade = new FrameFacadeAdapter(rootFrame);

                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(frameFacade, "AppFrame");

                //Initialize MvvmAppBase common services
                SuspensionManagerState = new SuspensionManagerState();
                NavigationService = CreateNavigationService(frameFacade, SuspensionManagerState);
                FlyoutService = new FlyoutService();
                FlyoutService.FlyoutResolver = CreateFlyoutView;
                // <snippet518>
                SettingsCharmService = new SettingsCharmService(GetSettingsCharmItems, FlyoutService);
                SettingsPane.GetForCurrentView().CommandsRequested += SettingsCharmService.OnCommandsRequested;
                // </snippet518>

                // Set a factory for the ViewModelLocator to use the default resolution mechanism to construct view models
                ViewModelLocator.SetDefaultViewModelFactory(Resolve);

                OnRegisterKnownTypesforSerialization();
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    await SuspensionManager.RestoreSessionStateAsync();
                }

                OnInitialize(args);

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state and navigate to the last page visited
                    try
                    {
                        SuspensionManager.RestoreFrameState();
                        NavigationService.RestoreSavedNavigation();
                    }
                    catch (SuspensionManagerException)
                    {
                        // Something went wrong restoring state.
                        // Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

        // <snippet403>
        private INavigationService CreateNavigationService(IFrameFacade rootFrame, ISuspensionManagerState suspensionManagerSessionState)
        {
            var sessionStateWrapper = new FrameSessionStateWrapper();

            var navigationService = new FrameNavigationService(rootFrame, sessionStateWrapper, GetPageType, suspensionManagerSessionState);
            return navigationService;
        }
        // </snippet403>
        // </snippet704>

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        // <snippet701>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            IsSuspending = true;
            try
            {
                var deferral = e.SuspendingOperation.GetDeferral();

                //Bootstrap inform navigation service that app is suspending.
                NavigationService.Suspending();

                // Save application state
                await SuspensionManager.SaveAsync();

                //TODO: Stop any background activity
                deferral.Complete();
            }
            finally
            {
                IsSuspending = false;
            }
        }
        // </snippet701>

        private void OnQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            var newArgs = new SearchEventArgs(args);
            OnSearchApplication(newArgs);
        }
    }
}
