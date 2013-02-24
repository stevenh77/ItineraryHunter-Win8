// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Windows.UI.Xaml;

namespace Kona.Infrastructure
{
    public static class ViewModelLocator
    {
        static Dictionary<string, Func<object>> factories = new Dictionary<string, Func<object>>();
        private static Func<Type, object> defaultViewModelFactory = type => Activator.CreateInstance(type);
        
        //Default View Type to VM Type resolver assumes VM is in same assembly and namespace as View Type.
        private static Func<Type, Type> defaultViewTypeToViewModelTypeResolver= 
            viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
                return Type.GetType(viewModelName);
            };

        public static void SetDefaultViewModelFactory(Func<Type, object> viewModelFactory)
        {
            defaultViewModelFactory = viewModelFactory;
        }

        public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
        {
            defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
        }

        #region Attached property with convention-or-mapping based approach

        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), 
            new PropertyMetadata(false, AutoWireViewModelChanged));

        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            if (obj != null)
            {
                return (bool) obj.GetValue(AutoWireViewModelProperty);
            }
            return false;
        }

        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            if (obj != null)
            {
                obj.SetValue(AutoWireViewModelProperty, value);
            }
        }

        #endregion

        // <snippet300>
        // <snippet3303>
        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement view = d as FrameworkElement;
            if (view == null) return; // Incorrect hookup, do no harm

            // Try mappings first
            object viewModel = GetViewModelForView(view);
            // Fallback to convention based
            if (viewModel == null)
            {
                var viewModelType = defaultViewTypeToViewModelTypeResolver(view.GetType());
                if (viewModelType == null) return;

                // Really need Container or Factories here to deal with injecting dependencies on construction
                viewModel = defaultViewModelFactory(viewModelType);
            }
            view.DataContext = viewModel;
        }
        // </snippet3303>
        // </snippet300>

        private static object GetViewModelForView(FrameworkElement view)
        {
            // Mapping of view models base on view type (or instance) goes here
            if (factories.ContainsKey(view.GetType().ToString()))
                return factories[view.GetType().ToString()]();
            return null;
        }

        public static void Register(string viewTypeName, Func<object> factory)
        {
            factories[viewTypeName] = factory;
        }
    }
}
