// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.Infrastructure.Flyouts;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.ViewModels
{
    public class SignInFlyoutViewModel : BindableBase, IFlyoutViewModel
    {
        private string _userName;
        private string _password;
        private bool _saveCredentials;
        private bool _isSignInInvalid;
        private readonly IAccountService _accountService;
        private Action _successAction;
        private UserInfo _lastSignedInUser;

        public SignInFlyoutViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            if (accountService != null)
            {
                _lastSignedInUser = _accountService.SignedInUser;
            }
            // <snippet308>
            SignInCommand = DelegateCommand.FromAsyncHandler(SignInAsync, CanSignIn);
            // </snippet308>
            GoBackCommand = new DelegateCommand(() => GoBack());
        }

        public string UserName
        {
            get
            {
                if (!IsNewSignIn)
                {
                    return _lastSignedInUser.UserName;
                }
                return _userName;
            }
            set
            {
                if (SetProperty(ref _userName, value))
                {
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsNewSignIn
        {
            get { return _lastSignedInUser == null; }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (SetProperty(ref _password, value))
                {
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool SaveCredentials
        {
            get { return _saveCredentials; }
            set { SetProperty(ref _saveCredentials, value); }
        }

        public bool IsSignInInvalid
        {
            get { return _isSignInInvalid; }
            set { SetProperty(ref _isSignInInvalid, value); }
        }

        public bool ExcludeFromSettingsPane
        {
            get { return _lastSignedInUser != null; }
        }
        public Action CloseFlyout { get; set; }
        

        public Action GoBack { get; set; }
        
        public DelegateCommand GoBackCommand { get; private set; }
        
        // <snippet309>
        public DelegateCommand SignInCommand { get; private set; }
        // </snippet309>

        public void Open(object parameter, Action successAction)
        {
            _successAction = successAction;
        }

        public bool CanSignIn()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        public async Task SignInAsync()
        {
            var result = await _accountService.SignInUserAsync(UserName, Password, useCredentialStore: SaveCredentials);

            if (result)
            {
                IsSignInInvalid = false;

                if (_successAction != null)
                {
                    _successAction();
                    _successAction = null;
                }

                CloseFlyout();
            }
            else
            {
                IsSignInInvalid = true;
            }
        }
    }
}
