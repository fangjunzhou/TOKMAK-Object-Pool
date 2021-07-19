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
            activeInstances = new Dictionary<T, float>();
            instances = new Queue<T>();
            this.processor = processor;

            // Create instances
            for (var i = 0; i < instanceCount; i++)
            {
                var obj = processor.CreateInstance(key, source);
                instances.Enqueue(obj);
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            // Destroy instances
            var instanceCount = instances.Count;
            for (var i = 0; i < instanceCount; i++) processor.DestroyInstance(key, instances.Dequeue());
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        public T source { get; }

        public Dictionary<T, float> activeInstances { get; }
        
        public Queue<T> instances { get; }

        public Processor<T> processor { get; }

        #endregion

        #region Instance

        public T GetInstance()
        {
            T value = default;

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
                    value = default;
                    return;
                }

                value = instances.Dequeue();
                
                // Add the instance to active list
                activeInstances.Add(value, Time.realtimeSinceStartup);
            }

            void GetNewInstance()
            {
                if (value != null)
                    return;
                
                value = processor.CreateInstance(key, source);
                
                // Add the instance to active list
                activeInstances.Add(value, Time.realtimeSinceStartup);
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
                T oldestInstance = default;
                foreach (T instances in activeInstances.Keys)
                {
                    float instanceTime = activeInstances[instances];
                    if (instanceTime < oldestTime)
                    {
                        oldestTime = instanceTime;
                        oldestInstance = instances;
                    }
                }
                
                value = oldestInstance;
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
            processor.OnEnableInstance(key, value);

            return value;
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
            
            processor.OnDisableInstance(key, value);
            activeInstances.Remove(value);
            instances.Enqueue(value);
        }

        #endregion
    }
}