
namespace CyDar.ServiceLocator
{
    public class TransientRegisteredType<T> : RegisteredTypeBase<T> where T : class
    {
        public override object Resolve(IServiceResolver resolver) => ResolveToConcrete.Invoke(resolver);
    }
}
