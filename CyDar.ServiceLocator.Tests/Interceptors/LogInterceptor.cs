using Castle.DynamicProxy;
using CyDar.ServiceLocator.Proxy;
using CyDar.ServiceLocator.Tests.Interfaces;

namespace CyDar.ServiceLocator.Tests.Interceptors
{
    class LogInterceptor : IIntercept
    {
        private readonly IServiceResolver resolver;

        public LogInterceptor(IServiceResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Intercept(IInvocation invocation)
        {
            var logger = resolver.Resolve<IDumbLog>();
            logger.Log("Log 1");
            logger.Log("Befor Method");
            invocation.Proceed();
            logger.Log("After Method");
        }
    }
}
