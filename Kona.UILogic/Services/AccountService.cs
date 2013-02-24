// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.Infrastructure.Interfaces;
using Kona.UILogic.Models;
using System.Security;
using System.Net.Http;

namespace Kona.UILogic.Services
{
    public class AccountService : IAccountService
    {
        public const string ServerCookieHeaderKey = "AccountService_ServerCookieHeader";
        public const string SignedInUserKey = "AccountService_SignedInUser";
        private const string UserNameKey = "AccountService_UserName";
        private const string PasswordKey = "AccountService_Password";

        private readonly IIdentityService _identityService;
        private readonly ISuspensionManagerState _suspensionManagerState;
        private readonly ICredentialStore _credentialStore;
        private string _serverCookieHeader;
        private UserInfo _signedInUser;
        private string _userName;
        private string _password;

        public AccountService(IIdentityService identityService, ISuspensionManagerState suspensionManagerState, ICredentialStore credentialStore)
        {
            _identityService = identityService;
            _suspensionManagerState = suspensionManagerState;
            _credentialStore = credentialStore;
            if (_suspensionManagerState != null)
            {
                if (_suspensionManagerState.SessionState.ContainsKey(ServerCookieHeaderKey))
                {
                    _serverCookieHeader = _suspensionManagerState.SessionState[ServerCookieHeaderKey] as string;
                }
                if (_suspensionManagerState.SessionState.ContainsKey(SignedInUserKey))
                {
                    _signedInUser = _suspensionManagerState.SessionState[SignedInUserKey] as UserInfo;
                }
                if (_suspensionManagerState.SessionState.ContainsKey(UserNameKey))
                {
                    _userName = _suspensionManagerState.SessionState[UserNameKey].ToString();
                }
                if (_suspensionManagerState.SessionState.ContainsKey(PasswordKey))
                {
                    _password = _suspensionManagerState.SessionState[PasswordKey].ToString();
                }
            }
        }

        public string ServerCookieHeader
        {
            get { return _serverCookieHeader; }
        }

        public UserInfo SignedInUser { get { return _signedInUser; } }

        /// <summary>
        /// Gets the current active user signed in the app.
        /// </summary>
        /// <returns>A Task that, when complete, retrieves an active user that is ready to be used for any operation against the service.</returns>
        public async Task<UserInfo> GetSignedInUserAsync()
        {
            try
            {
                // If user is logged in, verify that the session in the service is still active
                if (_signedInUser != null && _serverCookieHeader != null && await _identityService.VerifyActiveSessionAsync(_signedInUser.UserName, _serverCookieHeader))
                {
                    return _signedInUser;
                }
            }
            catch (SecurityException)
            {
                // User's session has expired.
            }

            // Attempt to sign in using credentials stored in session state
            // If succeeds, ask for a new active session
            if (_userName != null && _password != null)
            {
                if (await SignInUserAsync(_userName, _password, false))
                {
                    return _signedInUser;
                }
            }

            // Attempt to sign in using credentials stored locally
            // If succeeds, ask for a new active session
            var savedCredentials = _credentialStore.GetSavedCredentials("KonaRI");
            if (savedCredentials != null)
            {
                savedCredentials.RetrievePassword();
                if (await SignInUserAsync(savedCredentials.UserName, savedCredentials.Password, false))
                {
                    return _signedInUser;
                }
            }

            return null;
        }

        public async Task<bool> SignInUserAsync(string userName, string password, bool useCredentialStore)
        {
            try
            {
                // <snippet507>
                var result = await _identityService.LogOnAsync(userName, password);
                // </snippet507>

                UserInfo previousUser = _signedInUser;
                _signedInUser = result.UserInfo;

                // Save Server cookie & SignedInUser in the StateService
                _serverCookieHeader = result.ServerCookieHeader;
                _suspensionManagerState.SessionState[ServerCookieHeaderKey] = _serverCookieHeader;
                _suspensionManagerState.SessionState[SignedInUserKey] = _signedInUser;

                // Save username and password in state service
                _userName = userName;
                _password = password;
                _suspensionManagerState.SessionState[UserNameKey] = userName;
                _suspensionManagerState.SessionState[PasswordKey] = password;

                if (useCredentialStore)
                {
                    // Save credentials in the CredentialStore
                    _credentialStore.SaveCredentials("KonaRI", userName, password);
                }

                RaiseUserChanged(_signedInUser, previousUser);
                return true;
            }
            catch (HttpRequestException)
            {
                _serverCookieHeader = string.Empty;
                _suspensionManagerState.SessionState[ServerCookieHeaderKey] = _serverCookieHeader;
            }

            return false;
        }

        public event EventHandler<UserChangedEventArgs> UserChanged;

        private void RaiseUserChanged(UserInfo newUserInfo, UserInfo oldUserInfo)
        {
            var handler = UserChanged;
            if (handler != null)
            {
                handler(this, new UserChangedEventArgs(newUserInfo, oldUserInfo));
            }
        }

        public void SignOut()
        {
            var previousUser = _signedInUser;
            _signedInUser = null;
            _serverCookieHeader = null;

            // remove user from the CredentialStore, if any
            _credentialStore.RemovedSavedCredentials("KonaRI");

            RaiseUserChanged(_signedInUser, previousUser);
        }
    }
}
