using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CyDar.ServiceLocator.Proxy
{
    internal sealed class AttributeInterceptorSelector<T> : IInterceptorSelector
    {
        private IServiceResolver Resolver { get; set; }
        private IEnumerable<IIntercept> GlobalInterceptors { get; set; }

        public AttributeInterceptorSelector(IServiceResolver resolver, IEnumerable<IIntercept> globalInterceptors)
        {
            Resolver = resolver;
            GlobalInterceptors = globalInterceptors;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            //TODO: cache the attributes

            // attributes from the method
            var interfaceMethodAttributes = method.GetCustomAttributes<InterceptAttribute>(true);

            #region Find the corresponding method in the type
            MethodInfo typeMethod = null;
            if (method.DeclaringType.GetTypeInfo().IsInterface)
            {
                var map = type.GetTypeInfo().GetRuntimeInterfaceMap(method.DeclaringType);
                var methodIndex = -1;
                if (method.IsGenericMethod)
                {
                    methodIndex = Array.IndexOf(map.InterfaceMethods, method.GetGenericMethodDefinition());
                }
                else
                {
                    methodIndex = Array.IndexOf(map.InterfaceMethods, method);
                }
                typeMethod = methodIndex == -1 ? null : map.TargetMethods[methodIndex];
            }
            else if (method.DeclaringType.GetTypeInfo().IsAbstract)
            {
                //Possible dubble registering when typeMethod only in abstract class, this is handled by uniqe filters below
                typeMethod = type.GetMethods().Where(m => m.Name == method.Name && m.GetBaseDefinition().Equals(method)).FirstOrDefault();
            }
            #endregion

            // attributes from the method in type
            IEnumerable<InterceptAttribute> typeMethodAttributes = null;
            if (typeMethod != null)
            {
                typeMethodAttributes = typeMethod.GetCustomAttributes<InterceptAttribute>(true);
            }
            else
            {
                typeMethodAttributes = new List<InterceptAttribute>();
            }

            // attributes from the type
            var typeAttributes = type.GetTypeInfo().GetCustomAttributes<InterceptAttribute>(true);

            var request = new ProxyRequest
            {
                Resolver = Resolver,
                RequestForInterface = typeof(T),
                RequestForType = type,
                RequestForMethod = method
            };

            var allAttributes = interfaceMethodAttributes.Concat(typeMethodAttributes)
                                                         .Concat(typeAttributes);

            var uniqueInterceptorsFromAttributes = allAttributes.GroupBy(a => a.GetType())
                                                                .Select(g => g.First().GetInterceptor(request))
                                                                .ToList();

            var uniqueInterceptors = uniqueInterceptorsFromAttributes.Concat(GlobalInterceptors)
                                                                     .GroupBy(a => a.GetType())
                                                                     .Select(g => g.First())
                                                                     .ToArray();

            return uniqueInterceptors;
        }
    }
}
