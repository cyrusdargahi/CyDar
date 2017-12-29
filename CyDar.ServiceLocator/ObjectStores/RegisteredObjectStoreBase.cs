using CyDar.ServiceLocator.Interfaces;
using System;
using System.Collections.Generic;

namespace CyDar.ServiceLocator
{
    internal abstract class RegisteredObjectStoreBase : IRegisteredObjectStore
    {
        public abstract IDictionary<Type, IList<IRegisteredType>> RegisteredObjects { get; }
        public abstract IStorage Storage { get; }

        public void Add(Type key, IRegisteredType value)
        {
            if (!RegisteredObjects.ContainsKey(key))
            {
                RegisteredObjects.Add(key, new List<IRegisteredType>());

            }
            RegisteredObjects[key].Add(value);
        }

        public void Clear()
        {
            foreach (var key in RegisteredObjects)
            {
                foreach (var regType in key.Value)
                {
                    if (regType is ICleanable cleanableType)
                        cleanableType.Clean();
                }
            }
            RegisteredObjects.Clear();
            SetStorage(null);
        }

        public bool ContainsKey(Type key) => RegisteredObjects.ContainsKey(key);

        /// <summary>
        /// Returns -1 if the key does not exsist, otherwise the actual count
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Count(Type key)
        {
            if (RegisteredObjects.ContainsKey(key))
            {
                return RegisteredObjects[key].Count;
            }

            return -1;
        }

        /// <summary>
        /// Gets the list in the key if key exists, otherwise null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<IRegisteredType> Get(Type key)
        {
            if (RegisteredObjects.ContainsKey(key))
            {
                return RegisteredObjects[key];
            }

            return null;
        }

        public abstract void SetStorage(IStorage storage);

        public IStorage GetStorage() => Storage;
    }
}
