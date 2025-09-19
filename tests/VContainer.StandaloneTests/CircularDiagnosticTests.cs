using NUnit.Framework;
using VContainer.Diagnostics;
using VContainer.Internal;

namespace VContainer.Tests
{
    public class CircularDiagnosticTests
    {
        [Test]
        public void CircularDependencyDiagnosticTest()
        {
            var builder = new ContainerBuilder();
            builder.Diagnostics = DiagnositcsContext.GetCollector("testScope");
            builder.Register<HasCircularDependency1>(Lifetime.Transient);
            builder.Register<HasCircularDependency2>(Lifetime.Transient);

            var injector = InjectorCache.GetOrBuild(typeof(HasCircularDependencyMsg1));

            // Reflection mode can detect cyclic dependency at build time (sometimes).
            if (injector is not ReflectionInjector)
            {
                var resolver = builder.Build();
                var ex = Assert.Throws<VContainerCircularDependenciesException>(() => resolver.Resolve<HasCircularDependency1>());
                string expected =
                    "Circular dependency detected! " +
                    "VContainer.Tests.HasCircularDependency1 -> " +
                    "VContainer.Tests.HasCircularDependency2 -> " +
                    "VContainer.Tests.HasCircularDependency1";
                Assert.That(ex.Message, Is.EqualTo(expected));
            }
        }
    }
}