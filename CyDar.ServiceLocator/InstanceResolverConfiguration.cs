namespace CyDar.ServiceLocator
{
    public class InstanceResolverConfiguration
    {
#if !PORTABLE
        /// <summary>
        /// This can be used to enable/disable proxies
        /// </summary>
        public bool? EnableProxy { get; set; }
#endif

        public bool? EnableTransientMode { get; set; }

        public IServiceResolver TransientRoot { get; set; }

    }
}
