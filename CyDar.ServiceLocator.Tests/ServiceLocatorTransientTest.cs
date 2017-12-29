using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CyDar.ServiceLocator.Enums;
using CyDar.ServiceLocator.Interfaces;
using CyDar.ServiceLocator.Tests.Concretes;
using CyDar.ServiceLocator.Tests.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace CyDar.ServiceLocator.Tests
{
    [TestClass]
    public class ServiceLocatorTransientTest
    {
        private readonly ServiceLocator serviceLocator;
        private readonly ServiceLocator serviceLocator2;

        public ServiceLocatorTransientTest()
        {
            ServiceLocator.GlobalConfiguration = new ResolverConfiguration { EnableTransientMode = true };
            serviceLocator = new ServiceLocator();
            serviceLocator2 = new ServiceLocator();
        }

        [TestInitialize]
        public void TestInit()
        {
            serviceLocator.Clear();
            serviceLocator2.Clear();
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode")]
        public void ServiceLocator_Transient_Resolved_Types_Registered_As_Singelton_Should_Not_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Singelton);
            serviceLocator2.Register<IDummy>(_ => new Dummy(), Scope.Singelton);

            var expected = serviceLocator.Resolve<IDummy>();
            var actual = serviceLocator2.Resolve<IDummy>();

            Assert.AreNotSame(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode")]
        public void ServiceLocator_Transient_Resolved_Types_In_Different_Threads_Registered_As_Singelton_Should_Not_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Singelton);
            serviceLocator2.Register<IDummy>(_ => new Dummy(), Scope.Singelton);

            var expected = serviceLocator.Resolve<IDummy>();

            var task = Task.Run(async () =>
            {
                await Task.Yield();
                return serviceLocator2.Resolve<IDummy>();
            });
            task.Wait();

            Assert.AreNotSame(expected, task.Result);
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode")]
        public void ServiceLocator_Transient_Resolved_Types_In_Same_Thread_Registered_As_Thread_Should_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Thread);
            serviceLocator2.Register<IDummy>(_ => new Dummy(), Scope.Thread);

            var expected = serviceLocator.Resolve<IDummy>();
            var actual = serviceLocator2.Resolve<IDummy>();

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode")]
        public void ServiceLocator_Transient_Resolved_Types_In_Different_Threads_Registered_As_Thread_Should_Not_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Thread);
            serviceLocator2.Register<IDummy>(_ => new Dummy(), Scope.Thread);

            int threadId1 = Thread.CurrentThread.ManagedThreadId;
            int threadId2 = -2;

            var expected = serviceLocator.Resolve<IDummy>();
            var task = Task.Run(async () =>
            {
                await Task.Yield();
                threadId2 = Thread.CurrentThread.ManagedThreadId;
                return serviceLocator2.Resolve<IDummy>();
            });
            task.Wait();

            Assert.AreNotEqual(threadId2, threadId1);
            Assert.AreNotSame(expected, task.Result);
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode")]
        public void ServiceLocator_Transient_Resolved_Types_Registered_As_Transient_Should_Not_Be_Same()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Transient);
            serviceLocator2.Register<IDummy>(_ => new Dummy(), Scope.Transient);

            var expected = serviceLocator.Resolve<IDummy>();
            var actual = serviceLocator2.Resolve<IDummy>();

            Assert.AreNotSame(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode")]
        public void ServiceLocator_Transient_Resolved_Types_Registered_As_Custom_Should_Not_Be_Same()
        {
            serviceLocator.SetStorage(CreateStorage());
            serviceLocator2.SetStorage(CreateStorage());

            serviceLocator.Register<IDummy>(_ => new Dummy(), Scope.Custom);
            serviceLocator2.Register<IDummy>(_ => new Dummy(), Scope.Custom);

            var expected = serviceLocator.Resolve<IDummy>();
            var actual = serviceLocator2.Resolve<IDummy>();

            Assert.AreNotSame(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode")]
        public void ServiceLocator_Transient_Should_Not_Affect_Default_ServiceLocator()
        {
            serviceLocator.Register<IDummy>(_ => new Dummy(1));

            try
            {
                var actual = new ServiceLocator(new InstanceResolverConfiguration { EnableTransientMode = false }).Resolve<IDummy>();
                Assert.Fail("Did not throw exception");
            }
            catch { }

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
