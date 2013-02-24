// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class SignInFlyoutViewModelFixture
    {
        [TestMethod]
        public async Task FiringSignInCommand_Persists_Credentials_And_Turns_Invisible()
        {
            bool accountServiceSignInCalled = false;
            bool flyoutClosed = false;

            var accountService = new MockAccountService()
                {
                    SignInUserAsyncDelegate = (username, password, useCredentialStore) =>
                        {
                            Assert.AreEqual("TestUsername", username);
                            Assert.AreEqual("TestPassword", password);
                            Assert.IsTrue(useCredentialStore);
                            accountServiceSignInCalled = true;
                            return Task.FromResult(true);
                        }
                };

            var target = new SignInFlyoutViewModel(accountService)
                {
                    CloseFlyout = () => flyoutClosed = true,
                    UserName = "TestUsername",
                    Password = "TestPassword",
                    SaveCredentials = true
                };

            await target.SignInCommand.Execute();

            Assert.IsTrue(accountServiceSignInCalled);
            Assert.IsTrue(flyoutClosed);
        }

        [TestMethod]
        public async Task FiringSignInCommand_WithNotRememberPassword_DoesNotSaveInCredentialStore()
        {
            var accountService = new MockAccountService()
                {
                    SignInUserAsyncDelegate = (username, password, useCredentialStore) =>
                        {
                            Assert.IsFalse(useCredentialStore);
                            return Task.FromResult(true);
                        }
                };

            var target = new SignInFlyoutViewModel(accountService)
                {
                    CloseFlyout = () => Assert.IsTrue(true),
                    SaveCredentials = false
                };

            await target.SignInCommand.Execute();
        }

        [TestMethod]
        public async Task SuccessfulSignIn_CallsSuccessAction()
        {
            var successActionCalled = false;
            var accountService = new MockAccountService()
                {
                    SignInUserAsyncDelegate = (username, password, useCredentialStore) => Task.FromResult(true)
                };

            var target = new SignInFlyoutViewModel(accountService) {CloseFlyout = () => Task.Delay(0)};

            target.Open(null, () => { successActionCalled = true; });
            
            await target.SignInCommand.Execute();

            Assert.IsTrue(successActionCalled);
        }

        [TestMethod]
        public void UserName_ReturnsLastSignedInUser_IfAvailable()
        {
            var accountService = new MockAccountService()
                {
                    SignedInUser = new UserInfo { UserName = "TestUserName" }
                };

            var target = new SignInFlyoutViewModel(accountService);

            Assert.AreEqual("TestUserName", target.UserName);
            Assert.IsFalse(target.IsNewSignIn);
        }
    }
}
