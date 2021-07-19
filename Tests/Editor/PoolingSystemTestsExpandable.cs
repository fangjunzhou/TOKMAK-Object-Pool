using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace kTools.Pooling.Editor.Tests
{
    public sealed class PoolingSystemTestsExpandable
    {
        #region Constructors

        public PoolingSystemTestsExpandable()
        {
            m_Key = new GameObject("Key");
            m_Obj = new GameObject("Obj");
            m_InstanceCount = 1;
        }

        #endregion

        #region Setup

        [SetUp]
        public void SetUp()
        {
            // Ensure Pools are set up
            if (PoolingSystem.HasPool<GameObject>(m_Key))
                PoolingSystem.DestroyPool<GameObject>(m_Key);
            PoolingSystem.CreatePool(m_Key, m_Obj, m_InstanceCount, true);
        }

        #endregion

        #region Fields

        private readonly GameObject m_Key;
        private readonly GameObject m_Obj;
        private readonly int m_InstanceCount;

        #endregion

        #region Tests

        [Test]
        public void CanCreatePool()
        {
            // Execution
            var hasPool = PoolingSystem.HasPool<GameObject>(m_Key);

            // Result
            Assert.IsTrue(hasPool);
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void CanDestroyPool()
        {
            // Execution
            PoolingSystem.DestroyPool<GameObject>(m_Key);
            var hasPool = PoolingSystem.HasPool<GameObject>(m_Obj);

            // Result
            Assert.IsFalse(hasPool);
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void CanGetInstance()
        {
            // Setup
            GameObject instance;

            // Execution
            var getInstance = PoolingSystem.TryGetInstance(m_Key, out instance);

            // Result
            Assert.IsTrue(getInstance);
            Assert.IsNotNull(instance);
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void CanGetMultipleInstance()
        {
            // Setup
            GameObject instance;
            GameObject instance2;

            // Execution
            var getInstance = PoolingSystem.TryGetInstance(m_Key, out instance);
            var getInstance2 = PoolingSystem.TryGetInstance(m_Key, out instance2);

            // Result
            Assert.IsTrue(getInstance);
            Assert.IsTrue(getInstance2);
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance2);
            Assert.AreNotEqual(instance, instance2);
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void CanReturnInstance()
        {
            // Setup
            GameObject instance;
            GameObject instance2;

            // Execution
            PoolingSystem.TryGetInstance(m_Key, out instance);
            PoolingSystem.TryGetInstance(m_Key, out instance2);
            PoolingSystem.ReturnInstance(m_Key, instance);
            PoolingSystem.ReturnInstance(m_Key, instance2);

            // Result
            LogAssert.NoUnexpectedReceived();
        }

        #endregion
    }
}