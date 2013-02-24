// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Kona.UILogic.Services;
using System;
using Kona.UILogic.Models;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockIdentityService : IIdentityService
    {
        public Func<string, string, Task<LogOnResult>> LogOnAsyncDelegate { get; set; }
        public Func<string, string, Task<bool>> VerifyActiveSessionDelegate { get; set; }
 
        public Task<LogOnResult> LogOnAsync(string userId, string password)
        {
            return LogOnAsyncDelegate(userId, password);
        }

        public Task<bool> VerifyActiveSessionAsync(string userId, string serverCookieHeader)
        {
            return VerifyActiveSessionDelegate(userId, serverCookieHeader);
        }
    }
}
