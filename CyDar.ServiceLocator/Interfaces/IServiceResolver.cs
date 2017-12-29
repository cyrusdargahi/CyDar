using System;

namespace CyDar.ServiceLocator
{
    public interface IServiceResolver
    {
        T Resolve<T>() where T : class;
        object Resolve(Type type);
    }
}
