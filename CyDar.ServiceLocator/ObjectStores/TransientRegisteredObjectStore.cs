using CyDar.ServiceLocator.Interfaces;
using System;
using System.Collections.Generic;

namespace CyDar.ServiceLocator
{
    internal class TransientRegisteredObjectStore : RegisteredObjectStoreBase
    {
        protected readonly IDictionary<Type, IList<IRegisteredType>> mRegisteredObjects = new Dictionary<Type, IList<IRegisteredType>>();
        protected IStorage mStorage;

        public override IDictionary<Type, IList<IRegisteredType>> RegisteredObjects => mRegisteredObjects;
        public override IStorage Storage => mStorage;

        public override void SetStorage(IStorage storage) => mStorage = storage;
    }
}
