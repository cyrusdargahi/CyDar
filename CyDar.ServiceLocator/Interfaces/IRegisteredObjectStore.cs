using System;
using System.Collections.Generic;

namespace CyDar.ServiceLocator.Interfaces
{
    public interface IRegisteredObjectStore
    {
        bool ContainsKey(Type key);

        void Add(Type key, IRegisteredType value);

        /// <summary>
        /// Returns -1 if the key does not exsist, otherwise the actual count
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int Count(Type key);

        /// <summary>
        /// Gets the list in the key if key exists, otherwise null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IList<IRegisteredType> Get(Type key);

        void Clear();

        void SetStorage(IStorage storage);

        IStorage GetStorage();
    }
}
