namespace CyDar.ServiceLocator.Tests.Interfaces
{
    public interface IWithOverloadsGeneric
    {
        void Foo<T>()
            where T : class;

        void Foo<T, V>()
            where T : class
            where V : class;

        void Foo<T, V>(T t, V v)
            where T : class
            where V : class;

        void Foo<T, V>(V v, T t)
            where T : class
            where V : class;
    }
}
