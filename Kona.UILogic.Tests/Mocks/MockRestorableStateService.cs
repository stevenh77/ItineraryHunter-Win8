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

namespace Kona.UILogic.Tests.Mocks
{
    public class MockRestorableStateService : IRestorableStateService
    {
        Dictionary<string, object> _stateBag = new Dictionary<string, object>();

        public void SaveState(string key, object state)
        {
            _stateBag[key] = state;
        }

        public object GetState(string key)
        {
            if (_stateBag.ContainsKey(key))
                return _stateBag[key];
            else
                return null;
        }

        public Task RestoreAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
