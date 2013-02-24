// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Windows.Security.Credentials;

namespace Kona.Infrastructure
{
    // <snippet504>
    public interface ICredentialStore
    {
        void SaveCredentials(string resource, string userName, string password);
        PasswordCredential GetSavedCredentials(string resource);
        void RemovedSavedCredentials(string resource);
    }
    // </snippet504>
}
