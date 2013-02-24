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
using System.Net;
using Kona.UILogic.Models;
using Windows.Security.Credentials;
using System.Net.Http;

namespace Kona.UILogic.Tests
{
    [TestClass]
    public class AccountServiceFixture
    {
        [TestMethod]
        public async Task SuccessfullSignIn_RaisesUserChangedEvent()
        {
            bool userChangedFired = false;

            var identityService = new MockIdentityService();
            var stateService = new MockRestorableStateService();
            identityService.LogOnAsyncDelegate = (userId, password) =>
                {
                    return Task.FromResult(new LogOnResult { ServerCookieHeader = string.Empty, UserInfo = new UserInfo{UserName = userId} });
                };

            var target = new AccountService(identityService, stateService, null);
            target.UserChanged += (sender, userInfo) => { userChangedFired = true; }; 

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword");
            Assert.IsTrue(retVal);
            Assert.IsTrue(userChangedFired);
        }

        [TestMethod]
        public async Task SuccessfullSignIn_SavesServerCookieHeader()
        {
            var identityService = new MockIdentityService();
            var stateService = new MockRestorableStateService();
            identityService.LogOnAsyncDelegate = (userId, password) =>
            {
                return Task.FromResult(new LogOnResult { ServerCookieHeader = "TestServerCookieHeader", UserInfo = new UserInfo { UserName = userId } });
            };

            var target = new AccountService(identityService, stateService, null);

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword");
            Assert.AreEqual("TestServerCookieHeader", stateService.GetState(AccountService.ServerCookieHeaderKey));
        }

        [TestMethod, Ignore]
        public async Task AccountService_ReadsServerCookieHeaderFromRestorableStateService()
        {
            var verifyActiveSessionCalled = false;
            var identityService = new MockIdentityService();
            var stateService = new MockRestorableStateService();
            stateService.SaveState(AccountService.ServerCookieHeaderKey, "SavedServerCookieHeader");
            identityService.VerifyActiveSessionDelegate = (userName, serverCookieHeader) =>
                                                              {
                                                                  Assert.AreEqual("SavedServerCookieHeader",
                                                                                  serverCookieHeader);
                                                                  verifyActiveSessionCalled = true;
                                                                  return Task.FromResult(true);
                                                              };

            var target = new AccountService(identityService, stateService, null);

            var retVal = await target.GetSignedInUserAsync();

            Assert.IsTrue(verifyActiveSessionCalled);
        }

        [TestMethod]
        public async Task FailedSignIn_DoesNotRaiseUserChangedEvent()
        {
            bool userChangedFired = false;

            var identityService = new MockIdentityService();
            var stateService = new MockRestorableStateService();
            identityService.LogOnAsyncDelegate = (userId, password) =>
            {
                throw new HttpRequestException();
            };

            var target = new AccountService(identityService, stateService, null);
            target.UserChanged += (sender, userInfo) => { userChangedFired = true; };

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword");
            Assert.IsFalse(retVal);
            Assert.IsFalse(userChangedFired);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsUserInfo_IfSessionIsStillLive()
        {
            var restorableStateService = new MockRestorableStateService();
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

            var target = new AccountService(identityService, restorableStateService, null);
            bool userSignedIn = await target.SignInUserAsync("TestUsername", "password");

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
            var restorableStateService = new MockRestorableStateService();
            var target = new AccountService(identityService, restorableStateService, credentialStore);

            var userInfo = await target.GetSignedInUserAsync(); 
            
            Assert.IsNull(userInfo);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsUserInfo_IfSessionIsStillInactiveButHasSavedCredentials()
        {
            var mockRestorableStateService = new MockRestorableStateService();
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
            var target = new AccountService(identityService, mockRestorableStateService, credentialStore);

            var userInfo = await target.GetSignedInUserAsync();

            Assert.IsNotNull(userInfo);
            Assert.AreEqual("ReturnedUserName", userInfo.UserName);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsNull_IfSessionIsStillInactiveAndHasInvalidSavedCredentials()
        {
            var restorableStateService = new MockRestorableStateService();
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
            var target = new AccountService(identityService, restorableStateService, credentialStore);

            var userInfo = await target.GetSignedInUserAsync();

            Assert.IsNull(userInfo);
        }

        [TestMethod]
        public async Task SignOut_RaisesUserChangedEvent()
        {
            var userChangedRaised = false;
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => null;
            var restorableStateService = new MockRestorableStateService();
            var target = new AccountService(null, restorableStateService, credentialStore);
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

        [TestMethod]
        public async Task GetSignedInUserAsync_WhenSessionTimedOut_AttemptsToAutoSignIn()
        {
            var userChangedEventRaised = false;
            var newUserName = string.Empty;
            var identityService = new MockIdentityService();
            identityService.LogOnAsyncDelegate = (s, s1) => Task.FromResult(new LogOnResult {UserInfo = new UserInfo{UserName = s}, ServerCookieHeader = string.Empty});
            var restorableStateService = new MockRestorableStateService();
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s =>
                                                              {
                                                                  if (s == "KonaRI")
                                                                  {
                                                                      return new PasswordCredential("KonaRI", "testusername",
                                                                                                    "testpassword");
                                                                  }
                                                                  return null;
                                                              };
            var target = new AccountService(identityService, restorableStateService, credentialStore);

            target.SignInUserAsync("testusername", "testpassword");
            target.UserChanged += (sender, args) =>
                                      {
                                          userChangedEventRaised = true;
                                          newUserName = args.NewUserInfo.UserName;
                                      };
            identityService.VerifyActiveSessionDelegate = (s, s1) =>
                                                              {
                                                                  throw new SecurityException();
                                                              };
            var userInfo = await target.GetSignedInUserAsync();

            Assert.IsTrue(userChangedEventRaised);
            Assert.AreEqual("testusername", newUserName);

        }
    }
}
