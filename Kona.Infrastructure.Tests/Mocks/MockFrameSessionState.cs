// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.Infrastructure.Interfaces;
using Windows.UI.Xaml.Controls;

namespace Kona.Infrastructure.Tests.Mocks
{
    public class MockFrameSessionState : IFrameSessionState
    {
        public Func<IFrameFacade, Dictionary<string, object>> GetSessionStateForFrameDelegate { get; set; }

        public Dictionary<string, object> GetSessionStateForFrame(IFrameFacade frame)
        {
            return this.GetSessionStateForFrameDelegate(frame);
        }
    }
}
