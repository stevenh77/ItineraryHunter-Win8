// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kona.WebServices.Models
{
    public class State
    {
        public State(IList<string> validZipCodeRanges)
        {
            ValidZipCodeRanges = validZipCodeRanges;
        }

        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public IList<string> ValidZipCodeRanges { get; private set; }
    }
}