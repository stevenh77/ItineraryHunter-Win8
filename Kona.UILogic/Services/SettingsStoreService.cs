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
using Windows.Storage;

namespace Kona.UILogic.Services
{
    public class SettingsStoreService : ISettingsStoreService
    {
        // <snippet500>
        private ApplicationDataContainer _settingsContainer;

        public SettingsStoreService() : this(ApplicationData.Current.RoamingSettings) { }

        public SettingsStoreService(ApplicationDataContainer applicationDataContainer)
        {
            _settingsContainer = applicationDataContainer;
        }
        // </snippet500>

        public T GetValue<T>(string container, string id)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", "container cannot be null");
            }

            if (id == null)
            {
                throw new ArgumentNullException("id", "id cannot be null");
            }

            ApplicationDataContainer dataContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            var value = dataContainer.Values[id];

            if (value == null) return default(T);

            return (T)value;
        }

        public IEnumerable<T> GetAllValues<T>(string container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", "container cannot be null");
            }

            ApplicationDataContainer dataContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            var values = new List<T>();

            foreach (var value in dataContainer.Values)
            {
                values.Add((T)value.Value);
            }

            return values;
        }

        public T GetEntity<T>(string container, string id) where T : new()
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", "container cannot be null");
            }

            if (id == null)
            {
                throw new ArgumentNullException("id", "id cannot be null");
            }

            ApplicationDataContainer dataContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            var value = dataContainer.Values[id];

            if (value == null) return default(T);

            return PopulateEntity<T>((ApplicationDataCompositeValue)value);
        }

        public IEnumerable<T> GetAllEntities<T>(string container) where T : new()
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", "container cannot be null");
            }

            ApplicationDataContainer dataContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            var values = new List<T>();

            foreach (var compositeValue in dataContainer.Values)
            {
                var entity = PopulateEntity<T>((ApplicationDataCompositeValue)compositeValue.Value);
                values.Add(entity);
            }

            return values;
        }

        public void SaveValue<T>(string container, string id, T value)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", "container cannot be null");
            }

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "id cannot be null");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value", "value cannot be null");
            }

            ApplicationDataContainer dataContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            dataContainer.Values[id] = value;
        }

        // <snippet501>
        public void SaveEntity<T>(string container, string id, T value) where T : new()
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", "container cannot be null");
            }

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "id cannot be null");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value", "value cannot be null");
            }

            ApplicationDataContainer dataContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            ApplicationDataCompositeValue compositeValue = GetCompositeValue(value);

            dataContainer.Values[id] = compositeValue;
        }
        // </snippet501>

        public void DeleteSetting(string container, string id)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", "container cannot be null");
            }

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "id cannot be null");
            }

            var dataContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            dataContainer.Values.Remove(id);
        }

        public void DeleteContainer(string container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", "container cannot be null");
            }

            _settingsContainer.DeleteContainer(container);
        }

        private T PopulateEntity<T>(ApplicationDataCompositeValue compositeValue) where T : new()
        {
            var entity = new T();

            if (entity != null)
            {
                foreach (var keyValue in compositeValue)
                {
                    entity.GetType().GetRuntimeProperty(keyValue.Key).SetValue(entity, keyValue.Value);
                }
            }

            return entity;
        }

        private ApplicationDataCompositeValue GetCompositeValue(object entity)
        {
            var compositeValue = new ApplicationDataCompositeValue();
            foreach (var property in entity.GetType().GetRuntimeProperties().Where(p => p.PropertyType.GetTypeInfo().IsSerializable))
            {
                compositeValue[property.Name] = entity.GetType().GetRuntimeProperty(property.Name).GetValue(entity);
            }

            return compositeValue;
        }
    }
}