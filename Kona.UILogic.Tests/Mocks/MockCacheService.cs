// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockCacheService : ICacheService
    {
        public Func<string, Task<bool>> DataExistsAndIsValidAsyncDelegate { get; set; }
        public Func<string, object> GetDataDelegate { get; set; }
        public Func<string, object, Task> SaveDataAsyncDelegate { get; set; }
        public Func<Uri, Task<Uri>> SaveExternalDataAsyncDelegate { get; set; }

        private object getDataAsyncDelegate { get; set; }

        public Task<bool> DataExistsAndIsValidAsync(string cacheKey)
        {
            return this.DataExistsAndIsValidAsyncDelegate(cacheKey);
        }

        public Task<T> GetDataAsync<T>(string cacheKey)
        {
            var getDataAsyncDelegateResult = this.GetDataDelegate(cacheKey);

            var result = (T)getDataAsyncDelegateResult;
            return Task.FromResult(result) as Task<T>;
        }

        public Task SaveDataAsync<T>(string cacheKey, T content)
        {
            return this.SaveDataAsyncDelegate(cacheKey, content);
        }

        public Task<Uri> SaveExternalDataAsync(Uri dataUrl)
        {
            return this.SaveExternalDataAsyncDelegate(dataUrl); 
        }
    }
}
