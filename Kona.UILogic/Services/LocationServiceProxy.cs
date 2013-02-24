// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kona.Infrastructure;

namespace Kona.UILogic.Services
{
    public class LocationServiceProxy : ILocationService
    {
        private string _clientBaseUrl = string.Format("{0}/api/Location", Constants.ServerAddress);

        public async Task<IReadOnlyCollection<string>> GetStatesAsync()
        {
            using (var client = new HttpClient())
            {
                client.AddCurrentCultureHeader();
                var response = await client.GetAsync(_clientBaseUrl);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ReadOnlyCollection<string>>();
                return result;
            }
        }
    }
}
