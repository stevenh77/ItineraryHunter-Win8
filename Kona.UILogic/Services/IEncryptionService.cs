// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Kona.UILogic.Services
{
    public interface IEncryptionService
    {
        Task<IBuffer> EncryptMessage(string message);
        Task<string> DecryptMessage(IBuffer buffer);
    }
}