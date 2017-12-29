using CyDar.ServiceLocator.Tests.Attributes;
using CyDar.ServiceLocator.Tests.Interfaces;

namespace CyDar.ServiceLocator.Tests.Concretes
{
    [LogInterceptor2]
    public class DummyIntercepted : IDummyIntercepted
    {
        int value;

        public DummyIntercepted()
        {

        }

        public DummyIntercepted(int value)
        {
            this.value = value;
        }

        public int GetValue()
        {
            return value;
        }

        [LogInterceptor]
        public int GetValueIntercepted()
        {
            return value;
        }
    }
}
