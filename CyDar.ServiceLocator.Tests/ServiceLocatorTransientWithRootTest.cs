using CyDar.ServiceLocator.Tests.Concretes;
using CyDar.ServiceLocator.Tests.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CyDar.ServiceLocator.Tests
{
    [TestClass]
    public class ServiceLocatorTransientWithRootTest
    {
        private readonly ServiceLocator rootResolver;
        private readonly ServiceLocator transientResolver;

        public ServiceLocatorTransientWithRootTest()
        {
            rootResolver = new ServiceLocator(new InstanceResolverConfiguration
            {
                EnableTransientMode = true
            });

            transientResolver = new ServiceLocator(new InstanceResolverConfiguration
            {
                EnableTransientMode = true,
                TransientRoot = rootResolver
            });
        }

        [TestInitialize]
        public void TestInit()
        {
            rootResolver.Clear();
            transientResolver.Clear();
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode WithRoot")]
        public void ServiceLocator_Transient_WithRoot_Resolving_Not_Locally_Registered_Should_Resolve_Parent_Registered()
        {
            rootResolver.Register<IDummy>(_ => new Dummy());
            var actual = transientResolver.Resolve<IDummy>();

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode WithRoot")]
        public void ServiceLocator_Transient_WithRoot_Resolving_Locally_Registered_Should_Resolve_Most_Derived()
        {
            var expected = 2;
            rootResolver.Register<IDummy>(_ => new Dummy(expected - 1));
            transientResolver.Register<IDummy>(_ => new Dummy(expected));

            var actual = transientResolver.Resolve<IDummy>().GetValue();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode WithRoot")]
        public void ServiceLocator_Transient_WithRoot_Resolving_Locally_Registered_But_No_Match_Should_Resolve_Parent_Registered()
        {
            var expected = 1;
            var condition = new { x = 0 };
            rootResolver.Register<IDummy>(_ => new Dummy(expected));
            transientResolver.Register<IDummy>(_ => new Dummy(expected + 1)).When(_ => condition.x == 1);

            var actual = transientResolver.Resolve<IDummy>().GetValue();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("ServiceLocator TransientMode WithRoot")]
        public void ServiceLocator_Transient_WithRoot_Resolving_By_Parent_Should_Have_Derived_Reference()
        {
            rootResolver.Register<IDummy>(r => new Dummy(r));
            var actual = transientResolver.Resolve<IDummy>().GetResolver();

            Assert.AreSame(transientResolver, actual);
        }
    }
}
