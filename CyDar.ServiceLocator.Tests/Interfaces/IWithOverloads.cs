namespace CyDar.ServiceLocator.Tests.Interfaces
{
    public interface IWithOverloads
    {
        void Foo();
        void Foo(int bar);
        void Foo(double bar);
        void Foo(decimal bar);
        void Foo(int bar, decimal baz);
        void Foo(decimal bar, int baz);
        void Foo(decimal bar, decimal baz);
    }
}
