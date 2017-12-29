using System;

namespace CyDar.ServiceLocator.Interfaces
{
    public interface IRegisteredType : IWithCondition
    {
        Type TypeToResolve { get; }
        object Resolve(IServiceResolver resolver);
        bool MatchesCondition(IServiceResolver resolver);
    }
}
