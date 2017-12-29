using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CyDar.ServiceLocator.Enums;
using CyDar.ServiceLocator.Exceptions;
using CyDar.ServiceLocator.Interfaces;
using CyDar.ServiceLocator.Tests.Concretes;
using CyDar.ServiceLocator.Tests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyDar.ServiceLocator.Tests
{
    [TestClass]
    public class ServiceLocatorTest
    {
        private readonly ServiceLocator serviceLocator = new ServiceLocator();

        [TestInitialize]
        public void TestInit()
        {
            serviceLocator.Clear();
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Resolved_Types_Registered_As_Singelton_Should_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Singelton);

            var expected = serviceLocator.Resolve<IDummy>();
            var actual = serviceLocator.Resolve<IDummy>();

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Resolved_Types_In_Different_Threads_Registered_As_Singelton_Should_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Singelton);

            var expected = serviceLocator.Resolve<IDummy>();

            var task = Task.Run(async () =>
            {
                await Task.Yield();
                return serviceLocator.Resolve<IDummy>();
            });
            task.Wait();

            Assert.AreSame(expected, task.Result);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Resolved_Types_In_Same_Thread_Registered_As_Thread_Should_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Thread);

            var expected = serviceLocator.Resolve<IDummy>();
            var actual = serviceLocator.Resolve<IDummy>();

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Resolved_Types_In_Different_Threads_Registered_As_Thread_Should_Not_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Thread);

            var expected = serviceLocator.Resolve<IDummy>();

            var task = Task.Run(async () =>
            {
                await Task.Yield();
                return serviceLocator.Resolve<IDummy>();
            });
            task.Wait();

            Assert.AreNotSame(expected, task.Result);
        }

        [TestMethod]
        [TestCategory("ServiceLocator"), ExpectedException(typeof(InvalidOperationException))]
        public void ServiceLocator_Register_With_Scope_Thread_With_Condition_Should_Throw_Exception()
        {
            var conditionObject = new List<int> { 1 };
            serviceLocator.Register<IDummy>(_ => new Dummy(1), Scope.Thread).When(r => conditionObject.First() == 1);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Resolved_Types_Registered_With_Scope_Thread_With_Condition_Should_Return_Correct()
        {
            var conditionObject = new List<int> { 1 };
            serviceLocator.Register<IDummy>(_ => new Dummy(1)).When(r => conditionObject.First() == 1);
            serviceLocator.Register<IDummy>(_ => new Dummy(2), Scope.Thread);

            var expected = serviceLocator.Resolve<IDummy>();
            Assert.AreEqual(1, expected.GetValue());

            conditionObject[0] = 2;
            var actual = serviceLocator.Resolve<IDummy>();
            Assert.AreNotSame(expected, actual);
            Assert.AreEqual(2, actual.GetValue());
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Resolved_Types_Registered_As_Transient_Should_Not_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Transient);

            var expected = serviceLocator.Resolve<IDummy>();
            var actual = serviceLocator.Resolve<IDummy>();

            Assert.AreNotSame(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Resolved_Types_Registered_As_Custom_Should_Be_Same()
        {
            serviceLocator.SetStorage(CreateStorage());

            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Custom);

            var expected = serviceLocator.Resolve<IDummy>();
            var actual = serviceLocator.Resolve<IDummy>();

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Resolved_Types_Registered_With_Condition_Should_Return_Right_Instance_When_Condition_Is_Met()
        {
            var testObject = new List<int> { 0 };
            const int expected1 = 1001;
            const int expected2 = 1002;

            serviceLocator.Register<IDummy>(_ => new Dummy(expected1)).When(_ => testObject.First() == 0);
            serviceLocator.Register<IDummy>(_ => new Dummy(expected2)).When(_ => testObject.First() == 1);

            var dummy = serviceLocator.Resolve<IDummy>();

            var actual1 = dummy.GetValue();

            Assert.AreEqual(expected1, actual1);

            testObject.Clear();
            testObject.Add(1);

            dummy = serviceLocator.Resolve<IDummy>();

            var actual2 = dummy.GetValue();
            Assert.AreEqual(expected2, actual2);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Resolved_Types_Registered_With_Condition_In_Singelton_Scope_Should_Return_Right_Instance_When_Condition_Is_Met()
        {
            var testObject = new List<int> { 0 };
            const int expected1 = 1001;
            const int expected2 = 1002;

            serviceLocator.Register<IDummy>(_ => new Dummy(expected1), Scope.Singelton).When(_ => testObject.First() == 0);
            serviceLocator.Register<IDummy>(_ => new Dummy(expected2), Scope.Singelton).When(_ => testObject.First() == 1);

            var dummy = serviceLocator.Resolve<IDummy>();

            var actual1 = dummy.GetValue();

            Assert.AreEqual(expected1, actual1);

            testObject.Clear();
            testObject.Add(1);

            dummy = serviceLocator.Resolve<IDummy>();

            var actual2 = dummy.GetValue();
            Assert.AreEqual(expected2, actual2);
        }

        [TestMethod]
        [TestCategory("ServiceLocator"), ExpectedException(typeof(TypeCannotBeResolvedException))]
        public void ServiceLocator_Resolved_Types_Registered_With_Condition_Should_Throw_Exception_When_Condition_Is_Not_Met()
        {
            var testObject = new List<int> { 0 };

            serviceLocator.Register<IDummy>(_ => new Dummy()).When(_ => testObject.First() == 1);
            serviceLocator.Register<IDummy>(_ => new Dummy()).When(_ => testObject.First() == 2);

            serviceLocator.Resolve<IDummy>();
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_RegisterToResolveAs_Types_Should_Be_Resolved()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy());
            serviceLocator.RegisterToResolveAs<IDummier, IDummy>();

            Assert.IsNotNull(serviceLocator.Resolve<IDummier>());
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_RegisterToResolveAs_Types_In_Singelton_Scope_Should_Be_Same_When_Resolved()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Singelton);
            serviceLocator.RegisterToResolveAs<IDummier, IDummy>();

            var dummy = serviceLocator.Resolve<IDummy>();
            var dummier = serviceLocator.Resolve<IDummier>();

            Assert.AreSame(dummy, dummier);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_RegisterToResolveAs_Types_In_Custom_Scope_Should_Be_Same_When_Resolved()
        {
            serviceLocator.SetStorage(CreateStorage());

            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Custom);
            serviceLocator.RegisterToResolveAs<IDummier, IDummy>();

            var dummy = serviceLocator.Resolve<IDummy>();
            var dummier = serviceLocator.Resolve<IDummier>();

            Assert.AreSame(dummy, dummier);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_RegisterToResolveAs_Types_In_Transient_Scope_Should_Not_Be_Same_When_Resolved()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy());
            serviceLocator.RegisterToResolveAs<IDummier, IDummy>();

            var dummy = serviceLocator.Resolve<IDummy>();
            var dummier = serviceLocator.Resolve<IDummier>();

            Assert.AreNotSame(dummy, dummier);
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_RegisterToResolveAs_Types_And_Condition_Should_Be_Resolved_When_Condition_Is_Met()
        {
            var testObject = new List<int> { 0 };

            serviceLocator.Register<IDummy>(_ => new Dummy()).When(_ => testObject.First() == 0);
            serviceLocator.RegisterToResolveAs<IDummier, IDummy>();

            Assert.IsNotNull(serviceLocator.Resolve<IDummier>());
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_RegisterToResolveAs_Types_And_Condition_Should_Be_Resolved_To_Correct_Instance_When_Condition_Is_Met()
        {
            var testObject = new List<int> { 0 };

            serviceLocator.Register<IDummy>(_ => new Dummy(1001)).When(_ => testObject.First() == 0);
            serviceLocator.Register<IDummy>(_ => new Dummy(1002));
            serviceLocator.RegisterToResolveAs<IDummier, IDummy>();

            Assert.AreEqual(1001, serviceLocator.Resolve<IDummier>().GetValue());

            testObject[0] = 1;

            Assert.AreEqual(1002, serviceLocator.Resolve<IDummier>().GetValue());
        }

        [TestMethod]
        [TestCategory("ServiceLocator"), ExpectedException(typeof(TypeCannotBeResolvedException))]
        public void ServiceLocator_RegisterToResolveAs_Types_And_Condition_Should_Throw_Exception_When_Condition_Is_Not_Met()
        {
            var testObject = new List<int> { 0 };

            serviceLocator.Register<IDummy>(_ => new Dummy()).When(_ => testObject.First() == 1);
            serviceLocator.RegisterToResolveAs<IDummier, IDummy>();

            serviceLocator.Resolve<IDummier>();
        }

        [TestMethod]
        [TestCategory("ServiceLocator"), ExpectedException(typeof(TypeNotRegisteredException))]
        public void ServiceLocator_RegisterToResolveAs_Types_Should_Throw_Exception_When_Type_Is_Not_Registered()
        {
            serviceLocator.RegisterToResolveAs<IDummier, IDummy>();

            serviceLocator.Resolve<IDummier>();
        }

        [TestMethod]
        [TestCategory("ServiceLocator")]
        public void ServiceLocator_Should_Store_Registered_Types_Between_Instances()
        {
            var expected = new Dummy(1);
            serviceLocator.Register<IDummy>(_ => expected);

            var actual = new ServiceLocator().Resolve<IDummy>();

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.GetValue(), actual.GetValue());
        }

        private static IStorage CreateStorage()
        {
            var store = new Dictionary<Type, object>();
            var moq = new Mock<IStorage>();
            moq.Setup(f => f.StoreObject(It.IsAny<Type>(), It.IsAny<object>())).Callback((Type key, object value) => store.Add(key, value));
            moq.Setup(f => f.GetObject(It.IsAny<Type>())).Returns((Type key) => store.ContainsKey(key) ? store[key] : null);
            moq.Setup(f => f.RemoveObject(It.IsAny<Type>())).Callback((Type key) => store.Remove(key));

            return moq.Object;
        }
    }
}
