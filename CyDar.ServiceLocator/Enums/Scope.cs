
namespace CyDar.ServiceLocator.Enums
{
    public enum Scope
    {
        /// <summary>
        /// A new instance of the type will be created each time one is requested. This is the default scope if none is specified.
        /// </summary>
        Transient,
        /// <summary>
        /// Only a single instance of the type will be created, and the same instance will be returned for each subsequent request.
        /// </summary>
        Singelton,
        /// <summary>
        /// One instance of the type will be created per thread.
        /// </summary>
        Thread,
        Custom
    }
}
