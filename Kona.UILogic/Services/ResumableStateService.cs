// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kona.UILogic.Services
{
    public class RestorableStateService : IRestorableStateService
    {
        Dictionary<string, object> _stateBag;

        public void SaveState(string key, object state)
        {
            if (_stateBag != null && !string.IsNullOrWhiteSpace(key))
                _stateBag[key] = state;
        }

        public event EventHandler AppRestored = delegate { };

        public object GetState(string key)
        {
            if (_stateBag != null && _stateBag.ContainsKey(key))
                return _stateBag[key];
            return null;
        }

        public void RaiseAppRestored()
        {
            AppRestored(this,null);
        }

        public void SetFrameState(Dictionary<string, object> frameState)
        {
            _stateBag = frameState;
        }
    }
}
