using CyDar.ServiceLocator.Tests.Attributes;

namespace CyDar.ServiceLocator.Tests.Interfaces
{
    public interface IDummyInterceptedGeneric
    {
        int GetValue();

        /// <summary>
        /// Adds ToString length of t and v to value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="t"></param>
        /// <param name="v"></param>
        /// <returns>Sum of value, t.ToString().length, v.ToString().Length</returns>
        [LogInterceptor]
        int GetValueIntercepted<T, V>(T t, V v)
            where T : struct
            where V : struct;

        /// <summary>
        /// Multiplies ToString().Length of t and v with value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns>Product of value, t.ToString().length, v.ToString().Length</returns>
        [LogInterceptor2]
        int GetValueIntercepted<T, V>(V v, T t)
            where T : struct
            where V : struct;
    }
}
