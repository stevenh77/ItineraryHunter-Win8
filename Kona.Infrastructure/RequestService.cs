// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kona.Infrastructure
{
    public class RequestService : IRequestService
    {
        // <snippet517>
        public async Task<byte[]> GetExternalResourceAsync(Uri resourceUrl)
        {
            using (HttpClient request = new HttpClient())
            {
                HttpResponseMessage response = await request.GetAsync(resourceUrl);
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
        // </snippet517>
    }
}
