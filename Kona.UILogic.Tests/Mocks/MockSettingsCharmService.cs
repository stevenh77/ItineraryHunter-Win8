// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.Infrastructure;
namespace Kona.UILogic.Tests.Mocks
{
    public class MockSettingsCharmService : ISettingsCharmService
    {
        public Action<string, object, Action> ShowFlyoutDelegate { get; set; }

        public void ShowFlyout(string flyoutId)
        {
            ShowFlyoutDelegate(flyoutId, null, null);
        }

        public void ShowFlyout(string flyoutId, object parameter, System.Action successAction)
        {
            ShowFlyoutDelegate(flyoutId, parameter, successAction);
        }
    }
}
