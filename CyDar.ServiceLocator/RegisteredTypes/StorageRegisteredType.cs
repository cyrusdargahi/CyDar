using CyDar.ServiceLocator.Interfaces;
using System;

namespace CyDar.ServiceLocator
{
    public class StorageRegisteredType<T> : RegisteredTypeBase<T>, ICleanable where T : class
    {
        private readonly IStorage storage;

        public StorageRegisteredType(IStorage storage)
            : base()
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public override object Resolve(IServiceResolver resolver)
        {
            var concrete = storage.GetObject(TypeToResolve);
            if (concrete == null)
            {
                concrete = ResolveToConcrete.Invoke(resolver);
                storage.StoreObject(TypeToResolve, concrete);
            }

            return concrete;
        }

        public void Clean() => storage.RemoveObject(TypeToResolve);
    }
}
