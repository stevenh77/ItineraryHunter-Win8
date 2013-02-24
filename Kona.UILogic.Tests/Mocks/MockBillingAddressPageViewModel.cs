// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.ViewModels;
namespace Kona.UILogic.Tests.Mocks
{
    public class MockBillingAddressPageViewModel : IBillingAddressUserControlViewModel
    {
        public Func<bool> ValidateFormDelegate { get; set; }
        public Action ProcessFormDelegate { get; set; }

        public Address Address { get; set; }        

        public IReadOnlyCollection<ComboBoxItemValue> States { get; set; }

        public bool SetAsDefault { get; set; }

        public string FirstError { get; set; }

        public Infrastructure.BindableValidator Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Windows.Input.ICommand GoBackCommand
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Windows.Input.ICommand GoNextCommand
        {
            get { throw new System.NotImplementedException(); }
        }

        public string EntityId { get; set; }


        public void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, System.Collections.Generic.Dictionary<string, object> viewState)
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedFrom(System.Collections.Generic.Dictionary<string, object> viewState, bool suspending)
        {
            throw new System.NotImplementedException();
        }

        public void GoNext()
        {
            throw new System.NotImplementedException();
        }

        public void ProcessForm()
        {
            ProcessFormDelegate();
        }

        public bool ValidateForm()
        {
            return ValidateFormDelegate();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public bool IsEnabled { get; set; }

        public Task PopulateStatesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
