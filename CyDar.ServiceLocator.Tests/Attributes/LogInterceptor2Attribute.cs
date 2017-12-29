using CyDar.ServiceLocator.Proxy;
using CyDar.ServiceLocator.Tests.Interceptors;

namespace CyDar.ServiceLocator.Tests.Attributes
{
    class LogInterceptor2Attribute : InterceptAttribute
    {
        public override IIntercept GetInterceptor(IProxyRequest request)
        {
            return new LogInterceptor2(request.Resolver);
        }
    }
}
