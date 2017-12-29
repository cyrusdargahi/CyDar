using Microsoft.VisualStudio.TestTools.UnitTesting;
using CyDar.ServiceLocator.Enums;
using CyDar.ServiceLocator.Tests.BaseClasses;
using CyDar.ServiceLocator.Tests.Concretes;
using CyDar.ServiceLocator.Tests.Interceptors;
using CyDar.ServiceLocator.Tests.Interfaces;
using System.Linq;

namespace CyDar.ServiceLocator.Tests
{
    [TestClass]
    public class ServiceLocatorWithProxyTest
    {
        private readonly ServiceLocator serviceLocator;

        public ServiceLocatorWithProxyTest()
        {
            ServiceLocator.GlobalConfiguration = new ResolverConfiguration { EnableProxy = true };
            serviceLocator = new ServiceLocator();
        }

        [TestInitialize]
        public void TestInit()
        {
            serviceLocator.Clear();
            
            serviceLocator.Register<IDumbLog>(_ => new DumbLog(), Scope.Singelton);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Should_Yield_Equal_Result_When_In_Singelton_Scope()
        {
            serviceLocator.Register<IDummyIntercepted>(_ => new DummyIntercepted(12), Scope.Singelton);

            var expected = serviceLocator.Resolve<IDummyIntercepted>().GetValue();
            var actual = serviceLocator.Resolve<IDummyIntercepted>().GetValue();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Should_Resolve_Same_Instance_When_In_Singelton_Scope()
        {
            serviceLocator.Register<IDummyIntercepted>(_ => new DummyIntercepted(12), Scope.Singelton);

            var expected = serviceLocator.Resolve<IDummyIntercepted>();
            var actual = serviceLocator.Resolve<IDummyIntercepted>();

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Should_Handle_Concrete_Class_Registrationg()
        {
            serviceLocator.Register<DummyIntercepted>(_ => new DummyIntercepted(12));

            var sut = serviceLocator.Resolve<DummyIntercepted>();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Should_Handle_Base_Class_Registrationg()
        {
            serviceLocator.Register<DimwitInterceptedBase>(_ => new DimwitIntercepted(12));

            var sut = serviceLocator.Resolve<DimwitInterceptedBase>();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Base_Class_Should_Handle_Log_Before_And_After_Method_Call()
        {
            var log = serviceLocator.Resolve<IDumbLog>();
            serviceLocator.Register<DimwitInterceptedBase>(_ => new DimwitIntercepted(12));

            var expected = serviceLocator.Resolve<DimwitInterceptedBase>().GetValueIntercepted();

            Assert.AreEqual(3, log.LogMessages.Count);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Base_Class_Should_Handle_Log_Before_And_After_Method_Call_With_Dubble_Intercept()
        {
            var log = serviceLocator.Resolve<IDumbLog>();
            serviceLocator.Register<DimwitInterceptedBase>(_ => new DimwitIntercepted(12));

            var expected = serviceLocator.Resolve<DimwitInterceptedBase>().GetValueDubbleIntercepted();

            Assert.AreEqual(6, log.LogMessages.Count);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Base_Class_Should_Not_Log_Method_Call_With_Not_Abstract_Methods()
        {
            var log = serviceLocator.Resolve<IDumbLog>();
            serviceLocator.Register<DimwitInterceptedBase>(_ => new DimwitIntercepted(12));

            var expected = serviceLocator.Resolve<DimwitInterceptedBase>().GetValueNotAbstract(12);

            Assert.AreEqual(0, log.LogMessages.Count);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Base_Class_Should_Log_Method_Call_With_Virtual_Not_Abstract_Methods()
        {
            var log = serviceLocator.Resolve<IDumbLog>();
            serviceLocator.Register<DimwitInterceptedBase>(_ => new DimwitIntercepted(12));

            var expected = serviceLocator.Resolve<DimwitInterceptedBase>().GetValueVirtualNotAbstract(12);

            Assert.AreEqual(3, log.LogMessages.Count);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Should_Log_Before_And_After_Method_Call()
        {
            var log = serviceLocator.Resolve<IDumbLog>();
            serviceLocator.Register<IDummyIntercepted>(_ => new DummyIntercepted(12), Scope.Singelton);

            serviceLocator.Resolve<IDummyIntercepted>().GetValueIntercepted();

            Assert.AreEqual(6, log.LogMessages.Count);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy")]
        public void ServiceLocatorWithProxy_Should_Global_Log_Before_And_After_Method_Call()
        {
            serviceLocator.RegisterGlobalInterceptor(new GlobalInterceptor(serviceLocator));

            var log = serviceLocator.Resolve<IDumbLog>();
            serviceLocator.Register<IDummyIntercepted>(_ => new DummyIntercepted(12), Scope.Singelton);

            serviceLocator.Resolve<IDummyIntercepted>().GetValueIntercepted();

            Assert.AreEqual(8, log.LogMessages.Count);
            Assert.IsNotNull(log.LogMessages.FirstOrDefault(l => l.StartsWith("Global:")));
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Overloaded")]
        public void ServiceLocatorWithProxy_Should_Work_With_Zero_Parameter_Method_Overload()
        {
            serviceLocator.Register<IWithOverloads>(_ => new WithOverloads());
            serviceLocator.Resolve<IWithOverloads>().Foo();
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Overloaded")]
        public void ServiceLocatorWithProxy_Should_Work_With_One_Parameter_Method_Overload()
        {
            serviceLocator.Register<IWithOverloads>(_ => new WithOverloads());
            serviceLocator.Resolve<IWithOverloads>().Foo(42);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Overloaded")]
        public void ServiceLocatorWithProxy_Should_Work_With_Two_Parameters_Method_Overload()
        {
            serviceLocator.Register<IWithOverloads>(_ => new WithOverloads());
            serviceLocator.Resolve<IWithOverloads>().Foo(42, 42M);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Overloaded")]
        public void ServiceLocatorWithProxy_Should_Work_With_Two_Parameters_Oposite_Order_Method_Overload()
        {
            serviceLocator.Register<IWithOverloads>(_ => new WithOverloads());
            serviceLocator.Resolve<IWithOverloads>().Foo(42M, 42);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Overloaded")]
        public void ServiceLocatorWithProxy_Should_Work_With_Two_Parameters_Same_Type_Method_Overload()
        {
            serviceLocator.Register<IWithOverloads>(_ => new WithOverloads());
            serviceLocator.Resolve<IWithOverloads>().Foo(42M, 42M);
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Generic")]
        public void ServiceLocatorWithProxy_Should_Work_With_Two_Generic_Zero_Parameter_Method_Overload()
        {
            serviceLocator.Register<IWithOverloadsGeneric>(_ => new WithOverloadsGeneric());
            serviceLocator.Resolve<IWithOverloadsGeneric>().Foo<Dummy, Dummy>();
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Generic")]
        public void ServiceLocatorWithProxy_Should_Work_With_One_Generic_Zero_Parameter_Method_Overload()
        {
            serviceLocator.Register<IWithOverloadsGeneric>(_ => new WithOverloadsGeneric());
            serviceLocator.Resolve<IWithOverloadsGeneric>().Foo<Dummy>();
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Generic")]
        public void ServiceLocatorWithProxy_Should_Work_With_Two_Generic_Two_Parameter_Method_Overload()
        {
            serviceLocator.Register<IWithOverloadsGeneric>(_ => new WithOverloadsGeneric());
            serviceLocator.Resolve<IWithOverloadsGeneric>().Foo<Dummy, DummyIntercepted>(new Dummy(), new DummyIntercepted());
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Generic")]
        public void ServiceLocatorWithProxy_Should_Log_Using_Correct_Interceptor_When_Generic_Overloads()
        {
            var log = serviceLocator.Resolve<IDumbLog>();
            serviceLocator.Register<IDummyInterceptedGeneric>(_ => new DummyInterceptedGeneric(12), Scope.Singelton);

            serviceLocator.Resolve<IDummyInterceptedGeneric>().GetValueIntercepted<bool, int>(false, 23);

            Assert.AreEqual("Log 1", log.LogMessages.First());
        }

        [TestMethod]
        [TestCategory("ServiceLocatorWithProxy Generic")]
        public void ServiceLocatorWithProxy_Should_Log_Using_Correct_Interceptor_When_Generic_Overloads_Reverse_Params()
        {
            var log = serviceLocator.Resolve<IDumbLog>();
            serviceLocator.Register<IDummyInterceptedGeneric>(_ => new DummyInterceptedGeneric(12), Scope.Singelton);

            serviceLocator.Resolve<IDummyInterceptedGeneric>().GetValueIntercepted<bool, int>(23, false);

            Assert.AreEqual("Log 2", log.LogMessages.First());
        }

    }
}
