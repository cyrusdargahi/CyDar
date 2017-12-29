namespace CyDar.ServiceLocator
{
    public class ResolverConfiguration : IResolverConfiguration
    {
#if !PORTABLE
        /// <summary>
        /// This can be used to enable/disable proxies
        /// </summary>
        public bool EnableProxy { get; set; }
#endif

        public bool EnableTransientMode { get; set; }

        public IServiceResolver TransientRoot { get; set; }

        /// <summary>
        /// Default configurations for TypeResolver
        /// </summary>
        public static IResolverConfiguration Default
        {
            get
            {
#if !PORTABLE
                return new ResolverConfiguration { EnableTransientMode = false, EnableProxy = false };
#else
                return new ResolverConfiguration { EnableTransientMode = false };
#endif
            }
        }
    }
}
