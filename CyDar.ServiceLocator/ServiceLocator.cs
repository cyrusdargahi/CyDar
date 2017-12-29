using CyDar.ServiceLocator.Enums;
using CyDar.ServiceLocator.Exceptions;
using CyDar.ServiceLocator.Interfaces;
#if !PORTABLE
using CyDar.ServiceLocator.Proxy;
using Castle.DynamicProxy;
using System.Collections.Generic;
using System.Reflection;
#endif
using System;
using System.Linq;

namespace CyDar.ServiceLocator
{
    public class ServiceLocator : IServiceResolver, IServiceRegisterer
    {
        protected readonly IRegisteredObjectStore RegisteredObjectsStore;
#if !PORTABLE
        protected static readonly IList<IIntercept> globalInterceptors = new List<IIntercept>();
#endif
        protected readonly IResolverConfiguration configuration;

        protected static IResolverConfiguration globalConfiguration = ResolverConfiguration.Default;
        /// <summary>
        /// Global configuration, this can be overriden per instance via constructor
        /// </summary>
        public static IResolverConfiguration GlobalConfiguration
        {
            get { return globalConfiguration; }
            set { globalConfiguration = value; }
        }

        public ServiceLocator() : this(new InstanceResolverConfiguration()) { }

        public ServiceLocator(InstanceResolverConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            configuration = new ResolverConfiguration
            {
#if !PORTABLE
                EnableProxy = config.EnableProxy.GetValueOrDefault(globalConfiguration.EnableProxy),

#endif
                EnableTransientMode = config.EnableTransientMode.GetValueOrDefault(globalConfiguration.EnableTransientMode)
            };

            if (configuration.EnableTransientMode)
            {
                RegisteredObjectsStore = new TransientRegisteredObjectStore();
                configuration.TransientRoot = config.TransientRoot;
            }
            else
            {
                RegisteredObjectsStore = new GlobalRegisteredObjectStore();
                configuration.TransientRoot = null;
            }
        }

        public virtual IWithCondition Register<T>(Func<IServiceResolver, T> resolveToConcrete) where T : class
        {
            return Register<T>(resolveToConcrete, Scope.Transient);
        }

        public virtual IWithCondition Register<T>(Func<IServiceResolver, T> resolveToConcrete, Scope scope) where T : class
        {
            var typeToResolve = typeof(T);

            IRegisteredType registeredObject = null;
            switch (scope)
            {
                case Scope.Transient:
                    registeredObject = new TransientRegisteredType<T> { ResolveToConcrete = WrapResolveToConcrete<T>(resolveToConcrete) };
                    break;
                case Scope.Singelton:
                    registeredObject = new SingeltonRegisteredType<T> { ResolveToConcrete = WrapResolveToConcrete<T>(resolveToConcrete) };
                    break;
                case Scope.Thread:
                    registeredObject = new ThreadRegisteredType<T> { ResolveToConcrete = WrapResolveToConcrete<T>(resolveToConcrete) };
                    break;
                case Scope.Custom:
                    registeredObject = new StorageRegisteredType<T>(RegisteredObjectsStore.GetStorage()) { ResolveToConcrete = WrapResolveToConcrete<T>(resolveToConcrete) };
                    break;
                default:
                    registeredObject = new TransientRegisteredType<T> { ResolveToConcrete = WrapResolveToConcrete<T>(resolveToConcrete) };
                    break;
            }
            RegisteredObjectsStore.Add(typeToResolve, registeredObject);

            return registeredObject as IWithCondition;
        }

        /// <summary>
        /// Registers a type to be resolved by using the registered settings of another type
        /// </summary>
        /// <typeparam name="T">The type to be resolved</typeparam>
        /// <typeparam name="V">The type to be used to resolve <paramref name="T"/></typeparam>
        public virtual void RegisterToResolveAs<T, V>()
            where T : class
            where V : class
        {
            var typeToResolve = typeof(T);

            IRegisteredType registeredObject = new DependentRegisteredType<T, V>();

            RegisteredObjectsStore.Add(typeToResolve, registeredObject);
        }

        public virtual T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        public virtual object Resolve(Type typeToResolve)
        {
            return Resolve(typeToResolve, this);
        }

        internal virtual object Resolve(Type typeToResolve, IServiceResolver context)
        {
            if (!RegisteredObjectsStore.ContainsKey(typeToResolve) || RegisteredObjectsStore.Get(typeToResolve) == null || RegisteredObjectsStore.Count(typeToResolve) == 0)
            {
                if (configuration.TransientRoot != null)
                {
                    return (configuration.TransientRoot as ServiceLocator)?.Resolve(typeToResolve, context);
                }
                throw new TypeNotRegisteredException(typeToResolve);
            }

            var registered = RegisteredObjectsStore.Get(typeToResolve).FirstOrDefault(x => x.MatchesCondition(this));
            if (registered != null)
            {
                return registered.Resolve(context);
            }
            else
            {
                if (configuration.TransientRoot != null)
                {
                    return (configuration.TransientRoot as ServiceLocator)?.Resolve(typeToResolve, context);
                }
                throw new TypeCannotBeResolvedException(typeToResolve);
            }
        }

        /// <summary>
        /// This method should only be called while unit testing.
        /// </summary>
        public virtual void Clear()
        {
            globalConfiguration = ResolverConfiguration.Default;
            RegisteredObjectsStore.Clear();
#if !PORTABLE
            globalInterceptors.Clear();
#endif
        }

        public virtual void SetStorage(IStorage storage)
        {
            RegisteredObjectsStore.SetStorage(storage);
        }

#if !PORTABLE
        public virtual void RegisterGlobalInterceptor(IIntercept interceptor)
        {
            globalInterceptors.Add(interceptor);
        }
#endif

        private Func<IServiceResolver, T> WrapResolveToConcrete<T>(Func<IServiceResolver, T> resolveToConcrete) where T : class
        {
#if !PORTABLE
            if (!configuration.EnableProxy)
                return resolveToConcrete;

            var generator = new ProxyGenerator();
            ProxyGenerationOptions options = new ProxyGenerationOptions { Selector = new AttributeInterceptorSelector<T>(this, /* send in a copy */ globalInterceptors.ToList()) };
            return resolver =>
            {
                var concrete = resolveToConcrete.Invoke(resolver);
                // TODO: cache these...
                var additionalInterfaces = concrete.GetType().GetInterfaces();
                if (typeof(T).GetTypeInfo().IsInterface)
                {
                    return (T)generator.CreateInterfaceProxyWithTarget(typeof(T), additionalInterfaces, resolveToConcrete.Invoke(resolver), options);
                }
                else
                {
                    return (T)generator.CreateClassProxyWithTarget(typeof(T), additionalInterfaces, resolveToConcrete.Invoke(resolver), options);
                }
            };
#else
            return resolveToConcrete;
#endif
        }
    }
}
