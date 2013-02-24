// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Security;
using System.Threading.Tasks;
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Kona.UILogic.Models;
using Windows.Security.Credentials;
using System.Net.Http;

namespace Kona.UILogic.Tests.Services
{
    [TestClass]
    public class AccountServiceFixture
    {
        [TestMethod]
        public async Task SuccessfullSignIn_RaisesUserChangedEvent()
        {
            bool userChangedFired = false;

            var identityService = new MockIdentityService();
            var suspensionManagerState = new MockSuspensionManagerState();
            identityService.LogOnAsyncDelegate = (userId, password) =>
                {
                    return Task.FromResult(new LogOnResult { ServerCookieHeader = string.Empty, UserInfo = new UserInfo{UserName = userId} });
                };

            var target = new AccountService(identityService, suspensionManagerState, null);
            target.UserChanged += (sender, userInfo) => { userChangedFired = true; }; 

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword", false);
            Assert.IsTrue(retVal);
            Assert.IsTrue(userChangedFired);
        }

        [TestMethod]
        public async Task SuccessfullSignIn_SavesServerCookieHeader()
        {
            var identityService = new MockIdentityService();
            var suspensionManagerState = new MockSuspensionManagerState();
            identityService.LogOnAsyncDelegate = (userId, password) =>
                {
                    return Task.FromResult(new LogOnResult { ServerCookieHeader = "TestServerCookieHeader", UserInfo = new UserInfo { UserName = userId } });
                };

            var target = new AccountService(identityService, suspensionManagerState, null);

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword", false);
            Assert.AreEqual("TestServerCookieHeader", suspensionManagerState.SessionState[AccountService.ServerCookieHeaderKey]);
        }

        [TestMethod]
        public async Task GetSignedInUserAsync_Calls_VerifyActiveSessionAsync()
        {
            bool verifyActiveSessionCalled = false;
            var suspensionManagerState = new MockSuspensionManagerState();
            suspensionManagerState.SessionState[AccountService.ServerCookieHeaderKey] = "TestServerCookieHeader";
            var identityService = new MockIdentityService()
                {
                    LogOnAsyncDelegate = (userId, password) => Task.FromResult(new LogOnResult { ServerCookieHeader = "TestServerCookieHeader", UserInfo = new UserInfo { UserName = userId } }),
                    VerifyActiveSessionDelegate = (userName, serverCookieHeader) =>
                        {
                            Assert.AreEqual("TestServerCookieHeader", serverCookieHeader);
                            verifyActiveSessionCalled = true;
                            return Task.FromResult(true);
                        }
                };

            var target = new AccountService(identityService, suspensionManagerState, null);
            await target.SignInUserAsync("TestUserName", "TestPassword", false);
            var user = await target.GetSignedInUserAsync();

            Assert.IsTrue(verifyActiveSessionCalled);
            Assert.IsNotNull(user);
            Assert.IsTrue(user.UserName == "TestUserName");
        }

        [TestMethod]
        public async Task GetSignedInUserAsync_SignsInUsingCredentialStore_IfNoActiveSession()
        {
            var suspensionManagerState = new MockSuspensionManagerState();
            suspensionManagerState.SessionState[AccountService.ServerCookieHeaderKey] = "TestServerCookieHeader";
            var identityService = new MockIdentityService()
            {
                LogOnAsyncDelegate = (userId, password) => Task.FromResult(new LogOnResult { ServerCookieHeader = "TestServerCookieHeader", UserInfo = new UserInfo { UserName = userId } }),
                VerifyActiveSessionDelegate = (userName, serverCookieHeader) => Task.FromResult(false)
            };
            var credentialStore = new MockCredentialStore()
                {
                    GetSavedCredentialsDelegate = (s) => new PasswordCredential("KonaRI", "TestUserName", "TestPassword"),
                    SaveCredentialsDelegate = (a, b, c) => Task.Delay(0)
                };

            var target = new AccountService(identityService, suspensionManagerState, credentialStore);
            await target.SignInUserAsync("TestUserName", "TestPassword", true);
            
            var user = await target.GetSignedInUserAsync();

            Assert.IsNotNull(user);
            Assert.IsTrue(user.UserName == "TestUserName");
        }

        [TestMethod]
        public async Task FailedSignIn_DoesNotRaiseUserChangedEvent()
        {
            bool userChangedFired = false;

            var identityService = new MockIdentityService();
            var suspensionManagerState = new MockSuspensionManagerState();
            identityService.LogOnAsyncDelegate = (userId, password) =>
            {
                throw new HttpRequestException();
            };

            var target = new AccountService(identityService, suspensionManagerState, null);
            target.UserChanged += (sender, userInfo) => { userChangedFired = true; };

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword", false);
            Assert.IsFalse(retVal);
            Assert.IsFalse(userChangedFired);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsUserInfo_IfSessionIsStillLive()
        {
            var suspensionManagerState = new MockSuspensionManagerState();
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName, cookieHeader) => Task.FromResult(true);
            identityService.LogOnAsyncDelegate = (userName, password) =>
                {
                    return Task.FromResult(new LogOnResult()
                        {
                            ServerCookieHeader = "cookie",
                            UserInfo = new UserInfo() {UserName = "TestUsername"}
                        });
                };

            var target = new AccountService(identityService, suspensionManagerState, null);
            bool userSignedIn = await target.SignInUserAsync("TestUsername", "password", false);

            Assert.IsTrue(userSignedIn);

            var userInfo = await target.GetSignedInUserAsync();

            Assert.IsNotNull(userInfo);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsNull_IfSessionIsStillInactiveAndNoSavedCredentials()
        {
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName, cookieHeader) => Task.FromResult(false);
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => null;
            var suspensionManagerState = new MockSuspensionManagerState();
            var target = new AccountService(identityService, suspensionManagerState, credentialStore);

            var userInfo = await target.GetSignedInUserAsync(); 
            
            Assert.IsNull(userInfo);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsUserInfo_IfSessionIsStillInactiveButHasSavedCredentials()
        {
            var suspensionManagerState = new MockSuspensionManagerState();
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName, cookieHeader) => Task.FromResult(false);
            identityService.LogOnAsyncDelegate =
                (userName, password) =>
                    {
                        Assert.AreEqual("TestUserName", userName);
                        Assert.AreEqual("TestPassword", password);
                        return Task.FromResult(new LogOnResult()
                                            {
                                                UserInfo = new UserInfo(){UserName = "ReturnedUserName"}
                                            });
                    };
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => new PasswordCredential("KonaRI", "TestUserName", "TestPassword");
            var target = new AccountService(identityService, suspensionManagerState, credentialStore);

            var userInfo = await target.GetSignedInUserAsync();

            Assert.IsNotNull(userInfo);
            Assert.AreEqual("ReturnedUserName", userInfo.UserName);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsNull_IfSessionIsStillInactiveAndHasInvalidSavedCredentials()
        {
            var suspensionManagerState = new MockSuspensionManagerState();
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName, cookieHeader) => Task.FromResult(false);
            identityService.LogOnAsyncDelegate =
                (userName, password) =>
                {
                    Assert.AreEqual("TestUserName", userName);
                    Assert.AreEqual("TestPassword", password);
                    throw new HttpRequestException();
                };
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => new PasswordCredential("KonaRI", "TestUserName", "TestPassword");
            var target = new AccountService(identityService, suspensionManagerState, credentialStore);

            var userInfo = await target.GetSignedInUserAsync();

            Assert.IsNull(userInfo);
        }

        [TestMethod]
        public async Task SignOut_RaisesUserChangedEvent()
        {
            bool userChangedRaised = false;
            var suspensionManagerState = new MockSuspensionManagerState();
            var credentialStore = new MockCredentialStore
                {
                    GetSavedCredentialsDelegate = s => null,
                    RemovedSavedCredentialsDelegate = s => Task.Delay(0)
                };

            var target = new AccountService(null, suspensionManagerState, credentialStore);
            target.UserChanged += (sender, args) =>
                {
                    userChangedRaised = true;
                    Assert.IsNull(args.NewUserInfo);
                };

            target.SignOut();

            Assert.IsTrue(userChangedRaised);

            var signedInUser = await target.GetSignedInUserAsync();

            Assert.IsNull(signedInUser);
        }
    }
}
