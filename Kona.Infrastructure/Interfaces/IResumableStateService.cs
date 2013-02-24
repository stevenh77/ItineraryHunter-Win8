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

namespace Kona.Infrastructure
{
    public interface IRestorableStateService
    {
        void SaveState(string key, object state);
        event EventHandler AppRestored;
        object GetState(string key);
        void RaiseAppRestored();
        void SetFrameState(Dictionary<string,object> frameState);
    }
}
