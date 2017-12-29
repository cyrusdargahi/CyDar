using System;

namespace CyDar.ServiceLocator.Interfaces
{
    public interface IStorage
    {
        void StoreObject(Type key, object value);
        object GetObject(Type key);
        /// <summary>
        /// This method should only be used for unit testing to make 
        /// sure that all references and data are cleared on the instance.
        /// </summary>
        void RemoveObject(Type key);
    }
}
