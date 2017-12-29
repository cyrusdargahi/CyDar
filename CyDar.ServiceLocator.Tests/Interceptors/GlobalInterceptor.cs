using Castle.DynamicProxy;
using CyDar.ServiceLocator.Proxy;
using CyDar.ServiceLocator.Tests.Interfaces;

namespace CyDar.ServiceLocator.Tests.Interceptors
{
    class GlobalInterceptor : IIntercept
    {
        private readonly IServiceResolver resolver;

        public GlobalInterceptor(IServiceResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Intercept(IInvocation invocation)
        {
            var logger = resolver.Resolve<IDumbLog>();
            logger.Log("Global:Befor Method");
            invocation.Proceed();
            logger.Log("Global:After Method");
        }
    }
}
