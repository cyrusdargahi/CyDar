using CyDar.ServiceLocator.Interfaces;

namespace CyDar.ServiceLocator
{
    public class SingeltonRegisteredType<T> : RegisteredTypeBase<T>, ICleanable where T : class
    {
        private T instance;
        private object thisLock = new object();

        public override object Resolve(IServiceResolver resolver)
        {
            if (instance == null)
            {
                lock (thisLock)
                {
                    if (instance == null)
                    {
                        instance = ResolveToConcrete.Invoke(resolver);
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// This method should only be used for unit testing to make 
        /// sure that all references and data are cleared on the instance.
        /// </summary>
        public void Clean() => instance = null;
    }
}
