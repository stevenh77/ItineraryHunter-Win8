// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Kona.Infrastructure.Interfaces;

namespace Kona.Infrastructure
{
    public class SuspensionManagerState : ISuspensionManagerState
    {
        public Dictionary<string, object> SessionState
        {
            get { return SuspensionManager.SessionState; }
        }
    }
}
