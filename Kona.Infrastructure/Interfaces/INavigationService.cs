// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


namespace Kona.Infrastructure
{
    public interface INavigationService
    {
        bool Navigate(string pageToken, object parameter);
        void GoBack();
        bool CanGoBack();
        void ClearHistory();
        void RestoreSavedNavigation();
        void Suspending();
    }
}
