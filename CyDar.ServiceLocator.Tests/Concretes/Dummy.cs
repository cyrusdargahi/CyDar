using CyDar.ServiceLocator.Tests.Interfaces;

namespace CyDar.ServiceLocator.Tests.Concretes
{
    public class Dummy : IDummy, IDummier
    {
        int value;
        IServiceResolver resolver;
        public Dummy()
        {

        }

        public Dummy(int value)
        {
            this.value = value;
        }

        public Dummy(IServiceResolver resolver)
        {
            this.resolver = resolver;
        }

        public int GetValue()
        {
            return value;
        }

        public IServiceResolver GetResolver()
        {
            return resolver;
        }
    }
}
