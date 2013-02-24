// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;

// The Search Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace Kona.AWShopper.Views
{
    // TODO: Respond to activation for search results
    //
    // The following code could not be automatically added to your application subclass,
    // either because the appropriate class could not be located or because a method with
    // the same name already exists.  Ensure that appropriate code deals with activation
    // by displaying search results for the specified search term.
    //
    //         /// <summary>
    //         /// Invoked when the application is activated to display search results.
    //         /// </summary>
    //         /// <param name="args">Details about the activation request.</param>
    //         protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
    //         {
    //             // TODO: Register the Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
    //             // event in OnWindowCreated to speed up searches once the application is already running
    // 
    //             // If the Window isn't already using Frame navigation, insert our own Frame
    //             var previousContent = Window.Current.Content;
    //             var frame = previousContent as Frame;
    // 
    //             // If the app does not contain a top-level frame, it is possible that this 
    //             // is the initial launch of the app. Typically this method and OnLaunched 
    //             // in App.xaml.cs can call a common method.
    //             if (frame == null)
    //             {
    //                 // Create a Frame to act as the navigation context and associate it with
    //                 // a SuspensionManager key
    //                 frame = new Frame();
    //                 Kona.AWShopper.Views.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");
    // 
    //                 if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
    //                 {
    //                     // Restore the saved session state only when appropriate
    //                      try
    //                     {
    //                         await Kona.AWShopper.Views.Common.SuspensionManager.RestoreAsync();
    //                     }
    //                     catch (Kona.AWShopper.Views.Common.SuspensionManagerException)
    //                     {
    //                         //Something went wrong restoring state.
    //                         //Assume there is no state and continue
    //                     }
    //                 }
    //             }
    // 
    //             frame.Navigate(typeof(SearchResultsPage), args.QueryText);
    //             Window.Current.Content = frame;
    // 
    //             // Ensure the current window is active
    //             Window.Current.Activate();
    //         }
    /// <summary>
    /// This page displays search results when a global search is directed to this application.
    /// </summary>
    public sealed partial class SearchResultsPage : VisualStateAwarePage
    {

        public SearchResultsPage()
        {
            this.InitializeComponent();
        }

    }
}