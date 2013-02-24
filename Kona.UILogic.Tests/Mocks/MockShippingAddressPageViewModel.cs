// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.ViewModels;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockShippingAddressPageViewModel : IShippingAddressUserControlViewModel
    {
        public Address Address { get; set; }
        public IReadOnlyCollection<ComboBoxItemValue> States { get; set; }
        public bool SetAsDefault { get; set; }
        public string FirstError { get; set; }
        public BindableValidator Errors { get; private set; }
        public ICommand GoBackCommand { get; private set; }
        public ICommand GoNextCommand { get; private set; }
        public string EntityId { get; set; }
        public Func<bool> ValidateFormDelegate { get; set; }

        public Action ProcessFormDelegate { get; set; }

        public void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            throw new NotImplementedException();
        }

        public void GoNext()
        {
            throw new NotImplementedException();
        }

        public void ProcessForm()
        {
            ProcessFormDelegate();
        }

        public bool ValidateForm()
        {
            return ValidateFormDelegate();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Task PopulateStatesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
