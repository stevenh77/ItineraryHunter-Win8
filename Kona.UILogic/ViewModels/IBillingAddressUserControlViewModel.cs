// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public interface IBillingAddressUserControlViewModel
    {
        [RestorableState]
        Address Address { get; set; }

        IReadOnlyCollection<ComboBoxItemValue> States { get; set; }
        string EntityId { get; set; }
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState);
        void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending);
        void ProcessForm();
        bool ValidateForm();
        Task PopulateStatesAsync();
        event PropertyChangedEventHandler PropertyChanged;

        bool IsEnabled { get; set; }
    }
}