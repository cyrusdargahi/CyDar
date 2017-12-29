using CyDar.ServiceLocator.Tests.Attributes;
using CyDar.ServiceLocator.Tests.Interfaces;

namespace CyDar.ServiceLocator.Tests.Concretes
{
    [LogInterceptor2]
    public class DummyInterceptedGeneric : IDummyInterceptedGeneric
    {
        int value;

        public DummyInterceptedGeneric()
        {

        }

        public DummyInterceptedGeneric(int value)
        {
            this.value = value;
        }

        public int GetValue()
        {
            return value;
        }

        /// <summary>
        /// Adds ToString length of t and v to value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="t"></param>
        /// <param name="v"></param>
        /// <returns>Sum of value, t.ToString().length, v.ToString().Length</returns>
        public int GetValueIntercepted<T, V>(T t, V v)
            where T : struct
            where V : struct
        {
            return value + t.ToString().Length + v.ToString().Length;
        }

        /// <summary>
        /// Multiplies ToString().Length of t and v with value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns>Product of value, t.ToString().length, v.ToString().Length</returns>
        public int GetValueIntercepted<T, V>(V v, T t)
            where T : struct
            where V : struct
        {
            return value * t.ToString().Length * v.ToString().Length;
        }
    }
}
