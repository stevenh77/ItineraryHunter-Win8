// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved



using Microsoft.Practices.PubSubEvents;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Kona.Infrastructure.Tests.Events
{
    [TestClass]
    public class SubscriptionTokenFixture
    {
        [TestMethod]
        public void EqualsShouldReturnFalseIfSubscriptionTokenPassedIsNull()
        {
            SubscriptionToken token = new SubscriptionToken(t=> { });
            Assert.IsFalse(token.Equals(null));
        }

        [TestMethod]
        public void EqualsShouldReturnTrueWhenTokenIsTheSame()
        {
            SubscriptionToken token = new SubscriptionToken(t => { });
            Assert.IsTrue(token.Equals(token));
        }

        [TestMethod]
        public void EqualsShouldReturnTrueWhenComparingSameObjectInstances()
        {
            SubscriptionToken token = new SubscriptionToken(t => { });

            object tokenObject = token;

            Assert.IsTrue(token.Equals(tokenObject));
        }

        [TestMethod]
        public void EqualsShouldReturnFalseWhenComparingDifferentObjectInstances()
        {
            SubscriptionToken token = new SubscriptionToken(t => { });

            object tokenObject = new SubscriptionToken(t => { });

            Assert.IsFalse(token.Equals(tokenObject));
        }

        [TestMethod]
        public void HashCodeIsTheSameForSameToken()
        {
            SubscriptionToken token = new SubscriptionToken(t => { });
            int hashCode = token.GetHashCode();

            Assert.AreNotEqual(0, hashCode);
            Assert.AreEqual(hashCode, token.GetHashCode());
        }

        [TestMethod]
        public void WhenSubscriptionTokenIsDisposed_ThenEventUnSubscribes()
        {
            bool unsubscribed = false;

            SubscriptionToken token = new SubscriptionToken(t => { unsubscribed = true; });

            token.Dispose();

            Assert.IsTrue(unsubscribed);
        }

        [TestMethod]
        public void WhenDisposeIsCalledMoreThanOnce_ThenExceptionIsNotThrown()
        {
            SubscriptionToken token = new SubscriptionToken(t => { });

            token.Dispose();
            token.Dispose();
        }
    }
}