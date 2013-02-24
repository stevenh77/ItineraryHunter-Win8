// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kona.Infrastructure.Interfaces
{
    public interface IValidatableBindableBase : INotifyPropertyChanged
    {
        bool IsValidationEnabled { get; set; }
        BindableValidator Errors { get; }
        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors();
        bool ValidateProperties();
        void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors);
    }
}