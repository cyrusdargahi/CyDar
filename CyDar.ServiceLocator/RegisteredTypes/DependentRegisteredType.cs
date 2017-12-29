
namespace CyDar.ServiceLocator
{
    public class DependentRegisteredType<T, V> : RegisteredTypeBase<T>
        where T : class
        where V : class
    {
        public override object Resolve(IServiceResolver resolver) => resolver.Resolve<V>();
    }
}
