
namespace CyDar.ServiceLocator
{
    public interface IResolverConfiguration
    {
#if !PORTABLE
        bool EnableProxy { get; set; }
#endif
        bool EnableTransientMode { get; set; }
        IServiceResolver TransientRoot { get; set; }
    }
}
