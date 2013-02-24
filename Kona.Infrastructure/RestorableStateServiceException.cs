// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;

namespace Kona.Infrastructure
{
    public class RestorableStateServiceException : Exception
    {
        public RestorableStateServiceException()
        {
        }

        public RestorableStateServiceException(Exception e)
            : base("RestorableStateService failed", e)
        {
        }
    }
}
