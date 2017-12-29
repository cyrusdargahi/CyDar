using CyDar.ServiceLocator.Tests.Attributes;

namespace CyDar.ServiceLocator.Tests.BaseClasses
{
    public abstract class DimwitInterceptedBase
    {
        public abstract int GetValue();

        [LogInterceptor]
        public abstract int GetValueIntercepted();

        [LogInterceptor]
        public abstract int GetValueDubbleIntercepted();

        [LogInterceptor]
        public int GetValueNotAbstract(int value)
        {
            return value;
        }

        [LogInterceptor]
        public virtual int GetValueVirtualNotAbstract(int value)
        {
            return value;
        }
    }
}
