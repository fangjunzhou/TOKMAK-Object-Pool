using System;
using System.Collections.Generic;
using UnityEngine;

namespace kTools.Pooling
{
    internal abstract class Pool
    {
        #region Fields

        #endregion

        #region Constructors

        public Pool(object key, bool expandable)
        {
            this.key = key;
            this.expandable = expandable;
        }

        #endregion

        #region Properties

        public object key { get; }
        
        public bool expandable { get; }

        #endregion
    }

    internal sealed class Pool<T> : Pool, IDisposable
    {
        #region Constructors

        public Pool(object key, T source, int instanceCount, Processor<T> processor, bool expandable) : base(key, expandable)
        {
            // Set data
            this.source = source;
            activeInstances = new List<Instance<T>>();
            instances = new Queue<Instance<T>>();
            this.processor = processor;

            // Create instances
            for (var i = 0; i < instanceCount; i++)
            {
                var obj = processor.CreateInstance(key, source);
                var instance = new Instance<T>(obj);
                instances.Enqueue(instance);
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            // Destroy instances
            var instanceCount = instances.Count;
            for (var i = 0; i < instanceCount; i++) processor.DestroyInstance(key, instances.Dequeue().obj);
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        public T source { get; }

        public List<Instance<T>> activeInstances { get; }
        
        public Queue<Instance<T>> instances { get; }

        public Processor<T> processor { get; }

        #endregion

        #region Instance

        public T GetInstance()
        {
            Instance<T> value = null;

            // void GetInactiveInstance()
            // {
            //     if (value != null)
            //         return;
            //
            //     foreach (var instance in instances)
            //         if (!instance.activeSelf)
            //             value = instance;
            //     value = null;
            // }
            //
            // void GetOldestInstance()
            // {
            //     if (value != null)
            //         return;
            //
            //     var oldestTime = Mathf.Infinity;
            //     var oldestIndex = 0;
            //     for (var i = 0; i < instances.Length; i++)
            //         if (instances[i].activeTime < oldestTime)
            //         {
            //             oldestTime = instances[i].activeTime;
            //             oldestIndex = i;
            //         }
            //
            //     value = instances[oldestIndex];
            // }
            
            void GetInstanceFromQueue()
            {
                if (value != null)
                    return;

                if (instances.Count == 0)
                {
                    value = null;
                    return;
                }

                value = instances.Dequeue();
            }

            void GetNewInstance()
            {
                if (value != null)
                    return;
                
                var obj = processor.CreateInstance(key, source);
                value = new Instance<T>(obj);
            }

            void GetOldestInstance()
            {
                if (value != null)
                    return;
                
                if (activeInstances.Count == 0)
                {
                    throw new NullReferenceException("Both instances and activeInstances are null.");
                }

                var oldestTime = Mathf.Infinity;
                var oldestIndex = 0;
                for (var i = 0; i < activeInstances.Count; i++)
                    if (activeInstances[i].activeTime < oldestTime)
                    {
                        oldestTime = activeInstances[i].activeTime;
                        oldestIndex = i;
                    }
                
                value = activeInstances[oldestIndex];
            }

            // Get instance
            GetInstanceFromQueue();
            if (expandable)
            {
                GetNewInstance();
            }
            else
            {
                GetOldestInstance();
            }

            // Enable instance
            value.SetActive(true);
            processor.OnEnableInstance(key, value.obj);
            
            // Add the instance to active list
            activeInstances.Add(value);
            
            return value.obj;
        }

        public void ReturnInstance(T value)
        {
            // // Find instance 
            // foreach (var instance in instances)
            //     if (instance.obj.Equals(value))
            //     {
            //         // Disable instance
            //         instance.SetActive(false);
            //         processor.OnDisableInstance(key, instance.obj);
            //         return;
            //     }
            //
            // // Instance not tracked
            // Debug.LogWarning($"Pool ({key}) does not contain object ({value}).");

            for (int i = 0; i < activeInstances.Count; i++)
            {
                if (activeInstances[i].obj.Equals(value))
                {
                    activeInstances[i].SetActive(false);
                    processor.OnDisableInstance(key, activeInstances[i].obj);
                    activeInstances.RemoveAt(i);
                }
            }
            var instance = new Instance<T>(value);
            instances.Enqueue(instance);
        }

        #endregion
    }
}