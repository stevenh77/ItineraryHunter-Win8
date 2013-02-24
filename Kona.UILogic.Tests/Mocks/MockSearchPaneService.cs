// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure.Interfaces;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockSearchPaneService : ISearchPaneService
    {
        public void Show()
        {
        }

        public void ShowOnKeyboardInput(bool enable)
        {
        }

        public bool IsShowOnKeyBoardInputEnabled()
        {
            return true;
        }
    }
}
