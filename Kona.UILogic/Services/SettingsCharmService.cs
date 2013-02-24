// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Kona.Infrastructure.Flyouts;
using Windows.UI.ApplicationSettings;
using System.Linq;
using System;
using Kona.Infrastructure;

namespace Kona.UILogic.Services
{
    public class SettingsCharmService : ISettingsCharmService
    {
        private Func<IEnumerable<FlyoutView>> _flyoutsResolver;

        public SettingsCharmService(Func<IEnumerable<FlyoutView>> flyoutsResolver)
        {
            _flyoutsResolver = flyoutsResolver;
        }

        // <snippet519>
        public void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            if (args == null || args.Request == null || args.Request.ApplicationCommands == null)
            {
                return;
            }

            var applicationCommands = args.Request.ApplicationCommands;
            var flyouts = _flyoutsResolver();

            foreach (var flyout in flyouts)
            {
                var notFound = applicationCommands.FirstOrDefault(
                    (settingsCommand) => settingsCommand.Id.ToString() == flyout.CommandId) == null;
                if (notFound && !flyout.ExcludeFromSettingsPane)
                {
                    SettingsCommand cmd = new SettingsCommand(flyout.CommandId, flyout.CommandTitle,
                                                              (o) => flyout.Open(null, null));
                    applicationCommands.Add(cmd);
                }
            }
        }
        // </snippet519>

        public void ShowFlyout(string flyoutId, object parameter, Action successAction)
        {
            var flyouts = _flyoutsResolver();
            var flyout = flyouts.FirstOrDefault(c => c.CommandId == flyoutId);

            if (flyout != null)
            {
                flyout.Open(parameter, successAction);
            }
        }

        public void ShowFlyout(string flyoutId)
        {
            ShowFlyout(flyoutId, null, null);
        }
    }
}
