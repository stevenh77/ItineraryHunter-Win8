// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Practices.PubSubEvents
{
    internal class WeakDelegatesManager
    {
        private readonly List<DelegateReference> listeners = new List<DelegateReference>();

        public void AddListener(Delegate listener)
        {
            this.listeners.Add(new DelegateReference(listener, false));
        }

        public void RemoveListener(Delegate listener)
        {
            List<int> removals = new List<int>();
            for (int i = 0; i < listeners.Count; i++)
            {
                if (listeners[i].Target == listener || listeners[i] == null)
                    removals.Add(i);
            }
            foreach (var i in removals) listeners.RemoveAt(i);
        }

        public void Raise(params object[] args)
        {
            RemoveNulls();

            foreach (Delegate handler in this.listeners.ToList().Select(listener => listener.Target).Where(listener => listener != null))
            {
                handler.DynamicInvoke(args);
            }
        }

        private void RemoveNulls()
        {
            List<int> removals = new List<int>();
            for (int i = 0; i < listeners.Count; i++)
            {
                if (listeners[i].Target == null)
                    removals.Add(i);
            }
            foreach (var i in removals) listeners.RemoveAt(i);
        }
    }
}
