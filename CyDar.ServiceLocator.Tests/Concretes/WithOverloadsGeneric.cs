using CyDar.ServiceLocator.Tests.Interfaces;

namespace CyDar.ServiceLocator.Tests.Concretes
{
    public class WithOverloadsGeneric : IWithOverloadsGeneric
    {
        public void Foo<T>()
            where T : class
        { }

        public void Foo<T, V>()
            where T : class
            where V : class
        { }

        public void Foo<T, V>(T t, V v)
            where T : class
            where V : class
        { }

        public void Foo<T, V>(V v, T t)
            where T : class
            where V : class
        { }
    }
}
