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
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockShippingMethodService : IShippingMethodService
    {
        public Func<Task<IEnumerable<ShippingMethod>>> GetShippingMethodsAsyncDelegate {get;set;}
        public Func<Task<ShippingMethod>> GetBasicShippingMethodAsyncDelegate { get; set; }

        public Task<IEnumerable<ShippingMethod>> GetShippingMethodsAsync()
        {
            return GetShippingMethodsAsyncDelegate();
        }

        public Task<ShippingMethod> GetBasicShippingMethodAsync()
        {
            return GetBasicShippingMethodAsyncDelegate();
        }
    }
}
