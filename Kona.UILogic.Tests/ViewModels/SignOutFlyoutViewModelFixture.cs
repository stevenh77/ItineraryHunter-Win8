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
    public class SignOutFlyoutViewModelFixture
    {
        [TestMethod]
        public async Task SignOut_CallsSignOutinAccountServiceAndRemovesSavedCredentials()
        {
            bool closeFlyoutCalled = false;
            bool accountServiceSignOutCalled = false;
            bool clearHistoryCalled = false;
            bool navigateCalled = false;

            var accountService = new MockAccountService
                {
                    SignOutDelegate = () => accountServiceSignOutCalled = true,
                    GetSignedInUserAsyncDelegate = () => Task.FromResult(new UserInfo())
                };

            var navigationService = new MockNavigationService
                {
                    ClearHistoryDelegate = () => clearHistoryCalled = true,
                    NavigateDelegate = (s, o) =>
                        {
                            navigateCalled = true;
                            Assert.AreEqual("Hub", s);
                            return true;
                        }
                };

            var target = new SignOutFlyoutViewModel(accountService, navigationService) { CloseFlyout = () => closeFlyoutCalled = true };

            target.Open(null, null);
            await target.SignOutCommand.Execute();

            Assert.IsTrue(accountServiceSignOutCalled);
            Assert.IsTrue(closeFlyoutCalled);
            Assert.IsTrue(clearHistoryCalled);
            Assert.IsTrue(navigateCalled);
        }
    }
}
