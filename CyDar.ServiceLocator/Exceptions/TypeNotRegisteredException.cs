using System;

namespace CyDar.ServiceLocator.Exceptions
{
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(Type typeToResolve)
            : base(string.Format("The type {0} has not been registered for resolving, and therefore cannot be resolved.", typeToResolve.Name))
        {

        }
    }
}
