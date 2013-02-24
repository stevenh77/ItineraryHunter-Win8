// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.UILogic.Models;
using System;
using System.Globalization;

namespace Kona.UILogic.Services
{
    public class ModelValidationException : Exception
    {
        public ModelValidationException(ModelValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }

        public ModelValidationResult ValidationResult { get; set; }

        public override string Message
        {
            get
            {
                string result = string.Empty;
                bool firstItem = true;

                foreach (var key in ValidationResult.ModelState.Keys)
                {
                    if (!firstItem) result += "\n";

                    var errors = string.Join(", ", ValidationResult.ModelState[key].ToArray());
                    result += string.Format(CultureInfo.CurrentCulture, "{0} : {1}", key, errors);
                    firstItem = false;
                }

                return result;
            }
        }
    }
}
