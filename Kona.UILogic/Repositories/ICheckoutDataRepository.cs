// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Threading.Tasks;
using Kona.UILogic.Models;

namespace Kona.UILogic.Repositories
{
    public interface ICheckoutDataRepository
    {
        Address GetShippingAddress(string id);
        Address GetBillingAddress(string id);
        Task<PaymentMethod> GetPaymentMethodAsync(string id);

        Address GetDefaultShippingAddress();
        Address GetDefaultBillingAddress();
        Task<PaymentMethod> GetDefaultPaymentMethodAsync();

        IReadOnlyCollection<Address> GetAllShippingAddresses();
        IReadOnlyCollection<Address> GetAllBillingAddresses();
        Task<IReadOnlyCollection<PaymentMethod>> GetAllPaymentMethodsAsync();

        Address SaveShippingAddress(Address address);
        Address SaveBillingAddress(Address address);
        Task<PaymentMethod> SavePaymentMethodAsync(PaymentMethod paymentMethod);

        void SetDefaultShippingAddress(Address address);
        void SetDefaultBillingAddress(Address address);
        void SetDefaultPaymentMethod(PaymentMethod paymentMethod);

        void RemoveDefaultShippingAddress();
        void RemoveDefaultBillingAddress();
        void RemoveDefaultPaymentMethod();
    }
}