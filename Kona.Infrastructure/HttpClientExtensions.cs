// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Kona.Infrastructure
{
    public static class HttpClientExtensions
    {
        public static void AddCurrentCultureHeader(this HttpClient client)
        {
            if (client != null)
            {
                client.DefaultRequestHeaders.AcceptLanguage.Add(
                    new StringWithQualityHeaderValue(CultureInfo.CurrentUICulture.Name));
            }
        }
    }
}
