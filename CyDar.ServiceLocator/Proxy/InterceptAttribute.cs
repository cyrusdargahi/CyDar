using System;

namespace CyDar.ServiceLocator.Proxy
{
    public abstract class InterceptAttribute : Attribute
    {
        public abstract IIntercept GetInterceptor(IProxyRequest request);
    }
}
