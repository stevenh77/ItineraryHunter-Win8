// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure.Interfaces;
using Windows.ApplicationModel.Search;

namespace Kona.Infrastructure
{
    public class SearchPaneService : ISearchPaneService
    {
        public void Show()
        {
            SearchPane.GetForCurrentView().Show();
        }

        public void ShowOnKeyboardInput(bool enable)
        {
            SearchPane.GetForCurrentView().ShowOnKeyboardInput = enable;
        }

        public bool IsShowOnKeyBoardInputEnabled()
        {
            if (!AppManifestHelper.IsSearchDeclared())
            {
                return false;
            }

            return SearchPane.GetForCurrentView().ShowOnKeyboardInput;
        }
    }
}
