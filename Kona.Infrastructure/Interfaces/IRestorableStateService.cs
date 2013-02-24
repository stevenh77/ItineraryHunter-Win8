// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;

namespace Kona.Infrastructure.Interfaces
{
    public interface IRestorableStateService
    {
        void SaveState(string key, object state);
        object GetState(string key);
        Task RestoreAsync();
        Task SaveAsync();
    }
}
