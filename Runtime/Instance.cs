using UnityEngine;

namespace kTools.Pooling
{
    internal sealed class Instance<T>
    {
        #region Constructors

        public Instance(T obj)
        {
            this.obj = obj;
            SetActive(false);
        }

        #endregion

        #region State

        public void SetActive(bool value)
        {
            activeSelf = value;
            activeTime = value ? Time.realtimeSinceStartup : 0.0f;
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        public T obj { get; }

        public bool activeSelf { get; private set; }

        public float activeTime { get; private set; }

        #endregion
    }
}