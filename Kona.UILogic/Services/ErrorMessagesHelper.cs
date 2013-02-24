// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Windows.ApplicationModel.Resources;

namespace Kona.UILogic.Services
{
    public static class ErrorMessagesHelper
    {
        static ErrorMessagesHelper()
        {
            ResourceLoader = new ResourceLoaderAdapter(new ResourceLoader());    
        }
        public static IResourceLoader ResourceLoader { get; set; }

        public static string RequiredErrorMessage
        {
            get { return ResourceLoader.GetString("ErrorRequired"); }
        }

        public static string RegexErrorMessage
        {
            get { return ResourceLoader.GetString("ErrorRegex"); }
        }

        public static string CardNumberInvalidLengthErrorMessage
        {
            get { return ResourceLoader.GetString("ErrorCardNumberInvalidLength"); }
        }

        public static string CardMonthInvalid
        {
            get { return ResourceLoader.GetString("ErrorCardMonthInvalid"); }
        }

        public static string CardYearInvalid
        {
            get { return ResourceLoader.GetString("ErrorCardYearInvalid"); }
        }

        public static string GeneralServiceErrorMessage
        {
            get { return ResourceLoader.GetString("GeneralServiceErrorMessage"); }
        }

        // <snippet407>
        public static string ErrorProcessingOrder
        {
            get { return ResourceLoader.GetString("ErrorProcessingOrder"); }
        }
        // </snippet407>

        public static string Error
        {
            get { return ResourceLoader.GetString("Error"); }
        }
        
        public static string ErrorServiceUnreachable
        {
            get { return ResourceLoader.GetString("ErrorServiceUnreachable"); }
        }
    }
}
