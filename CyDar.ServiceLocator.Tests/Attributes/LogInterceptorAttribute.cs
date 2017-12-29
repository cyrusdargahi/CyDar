using CyDar.ServiceLocator.Proxy;
using CyDar.ServiceLocator.Tests.Interceptors;

namespace CyDar.ServiceLocator.Tests.Attributes
{
    class LogInterceptorAttribute : InterceptAttribute
    {
        public override IIntercept GetInterceptor(IProxyRequest request)
        {
            return new LogInterceptor(request.Resolver);
        }
    }
}
