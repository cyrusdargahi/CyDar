using System;
using System.Reflection;

namespace CyDar.ServiceLocator.Proxy
{
    public sealed class ProxyRequest : IProxyRequest
    {
        public IServiceResolver Resolver
        {
            get;
            internal set;
        }

        public Type RequestForInterface
        {
            get;
            internal set;
        }

        public Type RequestForType
        {
            get;
            internal set;
        }

        public MethodInfo RequestForMethod
        {
            get;
            internal set;
        }
    }
}
