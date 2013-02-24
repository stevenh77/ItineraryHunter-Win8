// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


namespace Kona.UILogic.Models
{
    public class AddressAndPaymentInfo
    {
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
    }
}
