// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


namespace Kona.Infrastructure
{
    public class SettingsCharmItem
    {
        public SettingsCharmItem(string settingsCharmTitle, string flyoutName)
        {
            SettingsCharmTitle = settingsCharmTitle;
            FlyoutName = flyoutName;
        }

        public string SettingsCharmTitle { get; private set; }
        public string FlyoutName { get; protected set; }
    }
}
