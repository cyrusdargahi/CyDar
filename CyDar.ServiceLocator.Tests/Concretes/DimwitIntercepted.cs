using CyDar.ServiceLocator.Tests.Attributes;
using CyDar.ServiceLocator.Tests.BaseClasses;

namespace CyDar.ServiceLocator.Tests.Concretes
{
    public class DimwitIntercepted : DimwitInterceptedBase
    {
        int value;

        public DimwitIntercepted()
        {

        }

        public DimwitIntercepted(int value)
        {
            this.value = value;
        }

        public override int GetValue()
        {
            return value;
        }

        public override int GetValueIntercepted()
        {
            return value;
        }

        [LogInterceptor2]
        public override int GetValueDubbleIntercepted()
        {
            return value;
        }

        public int GetValueDubbleIntercepted(string a)
        {
            return value;
        }
    }
}
