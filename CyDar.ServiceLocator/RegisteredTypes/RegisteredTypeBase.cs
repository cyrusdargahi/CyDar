using CyDar.ServiceLocator.Interfaces;
using System;

namespace CyDar.ServiceLocator
{
    public abstract class RegisteredTypeBase<T> : IRegisteredType where T : class
    {
        public Type TypeToResolve { get; private set; }
        public Func<IServiceResolver, T> ResolveToConcrete { get; set; }
        public Func<IServiceResolver, bool> ResolveCondition { get; set; }

        public RegisteredTypeBase()
        {
            TypeToResolve = typeof(T);
        }

        public abstract object Resolve(IServiceResolver resolver);

        public virtual bool MatchesCondition(IServiceResolver resolver) => ResolveCondition?.Invoke(resolver) ?? true;

        public virtual void When(Func<IServiceResolver, bool> condition) => ResolveCondition = condition ?? throw new ArgumentNullException(nameof(condition));

    }
}
