using System;

namespace CyDar.ServiceLocator.Interfaces
{
    public interface IWithCondition
    {
        void When(Func<IServiceResolver, bool> condition);
    }
}
