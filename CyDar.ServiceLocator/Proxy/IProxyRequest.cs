using System;
using System.Reflection;

namespace CyDar.ServiceLocator.Proxy
{
    public interface IProxyRequest
    {
        IServiceResolver Resolver { get; }
        Type RequestForInterface { get; }
        Type RequestForType { get; }
        MethodInfo RequestForMethod { get; }
    }
}
