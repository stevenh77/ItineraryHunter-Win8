// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;

namespace Kona.UILogic.Services
{
    public interface ISettingsStoreService
    {
        T GetValue<T>(string container, string id);

        IEnumerable<T> GetAllValues<T>(string container);

        void SaveValue<T>(string container, string id, T value);

        T GetEntity<T>(string container, string id) where T : new();

        IEnumerable<T> GetAllEntities<T>(string container) where T: new();

        void SaveEntity<T>(string container, string id, T value) where T : new();
        
        void DeleteSetting(string container, string id);

        void DeleteContainer(string container);
    }
}