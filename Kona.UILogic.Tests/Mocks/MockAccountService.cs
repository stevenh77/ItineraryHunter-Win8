// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Services;
using Kona.UILogic.Models;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockAccountService : IAccountService
    {
        public Func<string> GetUserIdDelegate { get; set; }

        public Func<string, string, bool,  Task<bool>> SignInUserAsyncDelegate { get; set; }

        public Func<Task<UserInfo>> GetSignedInUserAsyncDelegate { get; set; } 

        public Action SignOutDelegate { get; set; }

        public async Task<bool> SignInUserAsync(string userName, string password, bool useCredentialStore)
        {
            return await this.SignInUserAsyncDelegate(userName, password, useCredentialStore);
        }

        public UserInfo SignedInUser { get; set; }

        public async Task<UserInfo> GetSignedInUserAsync()
        {
            return await GetSignedInUserAsyncDelegate();
        }

        public void RaiseUserChanged(UserInfo newUserInfo, UserInfo oldUserInfo)
        {
            UserChanged(this, new UserChangedEventArgs(newUserInfo, oldUserInfo));
        }

        public event EventHandler<UserChangedEventArgs> UserChanged;


        public string ServerCookieHeader
        {
            get { return string.Empty; }
        }

        public void SignOut()
        {
            SignOutDelegate();
        }
    }
}
