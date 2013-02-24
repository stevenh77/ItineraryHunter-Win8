// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public interface IPaymentMethodUserControlViewModel
    {
        [RestorableState]
        PaymentMethod PaymentMethod { get; set; }

        string EntityId { get; set; }
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState);
        void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending);
        void ProcessForm();
        bool ValidateForm();
        event PropertyChangedEventHandler PropertyChanged;
    }
}