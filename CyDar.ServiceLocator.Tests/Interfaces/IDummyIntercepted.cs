using CyDar.ServiceLocator.Tests.Attributes;

namespace CyDar.ServiceLocator.Tests.Interfaces
{
    public interface IDummyIntercepted
    {
        int GetValue();

        [LogInterceptor]
        int GetValueIntercepted();
    }
}
