// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Kona.Infrastructure.Interfaces;

namespace Kona.Infrastructure
{
    public class ValidatableBindableBase : BindableBase, IValidatableBindableBase
    {
        private readonly BindableValidator _bindableValidator;

        public ValidatableBindableBase()
        {
            _bindableValidator = new BindableValidator(this);
        }

        public bool IsValidationEnabled
        {
            get { return this._bindableValidator.IsValidationEnabled; }
            set { this._bindableValidator.IsValidationEnabled = value; }
        }

        public BindableValidator Errors
        {
            get
            {
                return _bindableValidator;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { _bindableValidator.ErrorsChanged += value; }

            remove { _bindableValidator.ErrorsChanged -= value; }
        }

        public ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors()
        {
            return _bindableValidator.GetAllErrors();
        }

        public bool ValidateProperties()
        {
            return _bindableValidator.ValidateProperties();
        }

        public void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors)
        {
            _bindableValidator.SetAllErrors(entityErrors);
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var result = base.SetProperty(ref storage, value, propertyName);

            if (result && !string.IsNullOrEmpty(propertyName))
            {
                if (_bindableValidator.IsValidationEnabled)
                {
                    _bindableValidator.ValidateProperty(propertyName);
                }
            }
            return result;
        }
    }
}
