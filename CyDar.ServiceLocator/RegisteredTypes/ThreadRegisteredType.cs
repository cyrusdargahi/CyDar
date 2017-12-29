using CyDar.ServiceLocator.Interfaces;
using System;

namespace CyDar.ServiceLocator
{
    /// <summary>
    /// This class is only useful if YOU control the ThreadPool and lifecycle of threads.
    /// This class SHOULD NOT be used in ASP.NET since YOU don't control the ThreadPool (IIS does)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadRegisteredType<T> : RegisteredTypeBase<T>, ICleanable where T : class
    {
        [ThreadStatic]
        private static T instance;

        public override object Resolve(IServiceResolver resolver)
        {
            if (instance == null)
                instance = ResolveToConcrete.Invoke(resolver);

            return instance;
        }

        /// <summary>
        /// This method should only be used for unit testing to make 
        /// sure that all references and data are cleared on the instance.
        /// </summary>
        public void Clean() => instance = null;


        public override void When(Func<IServiceResolver, bool> condition)
        {
            throw new InvalidOperationException("Registered types with thread scope cannot have when conditions.");
        }
    }
}
