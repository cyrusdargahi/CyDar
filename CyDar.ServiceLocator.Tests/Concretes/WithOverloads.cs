using CyDar.ServiceLocator.Tests.Interfaces;

namespace CyDar.ServiceLocator.Tests.Concretes
{
    public class WithOverloads : IWithOverloads
    {
        public void Foo() { }
        public void Foo(int bar) { }
        public void Foo(double bar) { }
        public void Foo(decimal bar) { }
        public void Foo(int bar, decimal baz) { }
        public void Foo(decimal bar, int baz) { }
        public void Foo(decimal bar, decimal baz) { }
    }
}
