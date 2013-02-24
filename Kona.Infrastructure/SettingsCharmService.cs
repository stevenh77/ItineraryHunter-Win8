// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Kona.Infrastructure.Interfaces;
using Windows.UI.ApplicationSettings;
using System.Linq;
using System;

namespace Kona.Infrastructure
{
    public class SettingsCharmService
    {
        private readonly Func<IList<SettingsCharmItem>> _getSettingsCharmItems;
        private readonly IFlyoutService _flyoutService;

        public SettingsCharmService(Func<IList<SettingsCharmItem>> getSettingsCharmItems, IFlyoutService flyoutService)
        {
            _getSettingsCharmItems = getSettingsCharmItems;
            _flyoutService = flyoutService;
        }

        // <snippet519>
        public void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            if (args == null || args.Request == null || args.Request.ApplicationCommands == null
                || _getSettingsCharmItems == null)
            {
                return;
            }

            var applicationCommands = args.Request.ApplicationCommands;
            var settingsCharmItems = _getSettingsCharmItems();

            foreach (var settingsCharmItem in settingsCharmItems)
            {
                var notFound = applicationCommands.FirstOrDefault(
                    (settingsCommand) => settingsCommand.Id.ToString() == settingsCharmItem.FlyoutName) == null;
                if (notFound)
                {
                    SettingsCommand cmd = new SettingsCommand(settingsCharmItem.FlyoutName,
                                                              settingsCharmItem.SettingsCharmTitle,
                                                              (o) =>
                                                              _flyoutService.ShowFlyout(settingsCharmItem.FlyoutName));
                                                                  
                    applicationCommands.Add(cmd);
                }
            }
        }
        // </snippet519>
    }
}
