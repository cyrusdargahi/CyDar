using CyDar.ServiceLocator.Enums;
using CyDar.ServiceLocator.Interfaces;
#if !PORTABLE
using CyDar.ServiceLocator.Proxy;
#endif
using System;

namespace CyDar.ServiceLocator
{
    public interface IServiceRegisterer
    {
        IWithCondition Register<T>(Func<IServiceResolver, T> resolveToConcrete) where T : class;
        IWithCondition Register<T>(Func<IServiceResolver, T> resolveToConcrete, Scope scope) where T : class;
        /// <summary>
        /// Registers a type to be resolved by using the registered settings of another type
        /// </summary>
        /// <typeparam name="T">The type to be resolved</typeparam>
        /// <typeparam name="V">The type to be used to resolve <paramref name="T"/></typeparam>
        void RegisterToResolveAs<T, V>()
            where T : class
            where V : class;
        void Clear();
        void SetStorage(IStorage storage);

#if !PORTABLE
        void RegisterGlobalInterceptor(IIntercept interceptor);
#endif
    }
}
