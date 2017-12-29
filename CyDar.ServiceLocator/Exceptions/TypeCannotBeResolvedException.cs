using System;

namespace CyDar.ServiceLocator.Exceptions
{
    public class TypeCannotBeResolvedException : Exception
    {
        public TypeCannotBeResolvedException(Type typeToResolve)
            : base(string.Format("The type {0} cannot be resolved, perhaps due to no condition is fulfilled.", typeToResolve.Name))
        {

        }
    }
}
