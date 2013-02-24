// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Navigation;

namespace Kona.Infrastructure
{
    public class ViewModel : BindableBase, INavigationAware
    {
        public string EntityId { get; set; }

        public ViewModel()
        {
            EntityId = GetType().ToString();
        }

        public virtual void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            if (viewState != null && viewState.ContainsKey(EntityId))
            {
                RestoreState(viewState[EntityId]);
            }
        }

        public virtual void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            if (viewState != null)
            {
                viewState[EntityId] = RetrieveState();
            }
        }

        public T RetrieveEntityStateValue<T>(string entityStateKey, IDictionary<string, object> viewState)
        {
            if (viewState != null && viewState.ContainsKey(EntityId) && ((IDictionary<string, object>)viewState[EntityId]).ContainsKey(entityStateKey))
            {
                return (T)((IDictionary<string, object>)viewState[EntityId])[entityStateKey];
            }

            return default(T);
        }

        public void AddEntityStateValue(string entityStateKey, object entityStateValue, IDictionary<string, object> viewState)
        {
            if (viewState != null && viewState.ContainsKey(EntityId))
            {
                var entityState = ((IDictionary<string, object>)viewState[EntityId]);

                if (entityState.ContainsKey(entityStateKey))
                {
                    entityState[entityStateKey] = entityStateValue;
                }
                else
                {
                    entityState.Add(entityStateKey, entityStateValue);
                }
            }
        }

        // <snippet703>
        private IDictionary<string, object> RetrieveState()
        {
            Dictionary<string, object> entityState = new Dictionary<string, object>();
            FillEntityState(entityState, this);

            return entityState;
        }
        // </snippet703>

        // <snippet706>
        private void RestoreState(object state)
        {
            var entityState = (IDictionary<string, object>)state;

            RestoreEntityState(entityState, this);
        }
        // </snippet706>

        private static void FillEntityState(IDictionary<string, object> entityState, object entity)
        {
            var entityProperties = entity.GetType().GetRuntimeProperties().Where(
                                                            c => c.GetCustomAttribute(typeof(RestorableStateAttribute)) != null);

            foreach (PropertyInfo propertyInfo in entityProperties)
            {
                entityState.Add(propertyInfo.Name, propertyInfo.GetValue(entity));
            }
        }

        private static void RestoreEntityState(IDictionary<string, object> entityState, object entity)
        {
            var entityProperties = entity.GetType().GetRuntimeProperties().Where(
                                                            c => c.GetCustomAttribute(typeof(RestorableStateAttribute)) != null);

            foreach (PropertyInfo propertyInfo in entityProperties)
            {
                if (entityState.ContainsKey(propertyInfo.Name))
                {
                    propertyInfo.SetValue(entity, entityState[propertyInfo.Name]);
                }
            }
        }
    }
}
