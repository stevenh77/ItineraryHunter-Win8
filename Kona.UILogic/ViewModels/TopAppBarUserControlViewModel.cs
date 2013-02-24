// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Windows.Input;
using Kona.Infrastructure;
namespace Kona.UILogic.ViewModels
{
    public class TopAppBarUserControlViewModel : BindableBase
    {
        // <snippet402>
        public TopAppBarUserControlViewModel(INavigationService navigationService)
        {
            HomeNavigationCommand = new DelegateCommand(() => navigationService.Navigate("Hub", null));
            ShoppingCartNavigationCommand = new DelegateCommand(() => navigationService.Navigate("ShoppingCart", null));
        }

        public DelegateCommand HomeNavigationCommand { get; private set; }
        public DelegateCommand ShoppingCartNavigationCommand { get; private set; }
        // </snippet402>
    }
}
