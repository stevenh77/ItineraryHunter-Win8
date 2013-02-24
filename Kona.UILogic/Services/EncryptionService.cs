// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace Kona.UILogic.Services
{
    public class EncryptionService : IEncryptionService
    {
        public async Task<IBuffer> EncryptMessage(string message)
        {
            var dataProtectionProvider = new DataProtectionProvider("LOCAL=user");

            // Encode the plaintext input message to a buffer.
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(message, BinaryStringEncoding.Utf8);

            // Encrypt the message.
            IBuffer buffProtected = await dataProtectionProvider.ProtectAsync(buffMsg);
            return buffProtected;
        }

        public async Task<string> DecryptMessage(IBuffer buffer)
        {
            if (buffer == null) return string.Empty;

            var dataProtectionProvider = new DataProtectionProvider("LOCAL=user");

            // Decrypt the message
            IBuffer buffUnProtected = await dataProtectionProvider.UnprotectAsync(buffer);

            // Decode the buffer to a plaintext message.
            string message = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffUnProtected);
            return message;
        }

    }
}
