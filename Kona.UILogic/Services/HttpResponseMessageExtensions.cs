// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.UILogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Kona.UILogic.Services
{
    public static class HttpResponseMessageExtensions
    {
        // <snippet913>
        public static async Task EnsureSuccessWithValidationSupportAsync(this HttpResponseMessage response)
        {
            // If BadRequest, see if it contains a validation payload
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ModelValidationResult result = null;
                try
                {
                    result = await response.Content.ReadAsAsync<Models.ModelValidationResult>();
                }
                catch { } // Fall through logic will take care of it
                if (result != null) throw new ModelValidationException(result);

            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new SecurityException();

            response.EnsureSuccessStatusCode(); // Will throw for any other service call errors
        }
        // </snippet913>
    }
}
