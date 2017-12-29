using CyDar.ServiceLocator.Interfaces;
using System;
using System.Collections.Generic;

namespace CyDar.ServiceLocator
{
    internal class GlobalRegisteredObjectStore : RegisteredObjectStoreBase
    {
        protected static readonly IDictionary<Type, IList<IRegisteredType>> mRegisteredObjects = new Dictionary<Type, IList<IRegisteredType>>();
        protected static IStorage mStorage;

        public override IDictionary<Type, IList<IRegisteredType>> RegisteredObjects => mRegisteredObjects;
        public override IStorage Storage => mStorage;

        public override void SetStorage(IStorage storage) => mStorage = storage;
    }
}
