// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;

namespace Kona.UILogic.Services
{
    public interface ICacheService
    {
        Task<bool> DataExistsAndIsValidAsync(string cacheKey);

        Task<T> GetDataAsync<T>(string cacheKey);

        Task SaveDataAsync<T>(string cacheKey, T content);

        Task<Uri> SaveExternalDataAsync(Uri dataUrl);
    }
}
