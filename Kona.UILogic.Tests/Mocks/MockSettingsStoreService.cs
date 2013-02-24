// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockSettingsStoreService : ISettingsStoreService
    {
        private readonly Dictionary<string, Dictionary<string, object>> _values = new Dictionary<string, Dictionary<string, object>>();
        
        public Func<string, string, object> GetValueDelegate { get; set; }
        public Func<string, IEnumerable<object>> GetAllValuesDelegate { get; set; }
        public Func<string, string, object> GetEntityDelegate { get; set; }
        public Func<string, IEnumerable<object>> GetAllEntitiesDelegate { get; set; }
        
        public T GetValue<T>(string container, string id)
        {
            return _values.ContainsKey(container) && _values[container].ContainsKey(id) ? (T)_values[container][id] : default(T);
        }

        public IEnumerable<T> GetAllValues<T>(string container)
        {
            return _values.ContainsKey(container) ? (IEnumerable<T>)_values[container].Values.Select(c => (T)c).ToList() : default(IEnumerable<T>);
        }

        public T GetEntity<T>(string container, string id) where T : new()
        {
            return GetValue<T>(container, id);
        }

        public IEnumerable<T> GetAllEntities<T>(string container) where T : new()
        {
            return GetAllValues<T>(container);
        }

        public void SaveValue<T>(string container, string id, T value)
        {
            if (!_values.ContainsKey(container)) _values.Add(container, new Dictionary<string, object>());
            
            if (_values[container].ContainsKey(id))
            {
                _values[container][id] = value;
            }
            else
            {
                _values[container].Add(id, value);
            }
        }

        public void SaveEntity<T>(string container, string id, T value) where T : new()
        {
            SaveValue<T>(container, id, value);
        }

        public void DeleteSetting(string container, string id)
        {
            if (_values.ContainsKey(container))
            {
                _values[container].Remove(id);
            }
        }

        public void DeleteContainer(string container)
        {
            if (_values.ContainsKey(container))
            {
                _values.Remove(container);
            }
        }
    }
}
