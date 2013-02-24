// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kona.Infrastructure.Interfaces;

namespace Kona.Infrastructure.Tests.Mocks
{
    public class MockSuspensionManagerState : ISuspensionManagerState
    {
        private Dictionary<string, object> _sessionState = new Dictionary<string, object>();
        public Dictionary<string, object> SessionState
        {
            get { return _sessionState; }
        }
    }
}
