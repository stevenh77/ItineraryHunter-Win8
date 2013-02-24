// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Services;
using Windows.Storage.Streams;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockEncryptionService : IEncryptionService
    {
        public Func<string, Task<IBuffer>> EncryptMessageDelegate { get; set; }
        public Func<IBuffer, Task<string>> DecryptMessageDelegate { get; set; }

        public Task<IBuffer> EncryptMessage(string message)
        {
            return EncryptMessageDelegate(message);
        }

        public Task<string> DecryptMessage(IBuffer buffer)
        {
            return DecryptMessageDelegate(buffer);
        }
    }
}
