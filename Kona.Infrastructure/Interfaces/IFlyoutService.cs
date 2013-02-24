// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.Infrastructure.Flyouts;

namespace Kona.Infrastructure.Interfaces
{
    public interface IFlyoutService
    {
        Func<string, FlyoutView> FlyoutResolver { get; set; }
        void ShowFlyout(string flyoutId);
        void ShowFlyout(string flyoutId, object parameter, Action successAction);
    }
}
