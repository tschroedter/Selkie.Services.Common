using System.Diagnostics.CodeAnalysis;
using AutoFixture.NUnit3;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Core2.Selkie.NUnit.Extensions;
using Core2.Selkie.Services.Common.Interfaces;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;

namespace Core2.Selkie.Services.Common.Tests.NUnit
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public sealed class ServiceProgramTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void ConstructorCallsInstallTest([NotNull] [Frozen] IWindsorContainer container,
                                                [NotNull] [Frozen] IWindsorInstaller installer,
                                                [NotNull] ServiceProgram program)
        {
            container.Received().Install(installer);
        }

        [Theory]
        [AutoNSubstituteData]
        public void ConstructorCreatesLoggerTest([NotNull] [Frozen] IWindsorContainer container,
                                                 [NotNull] ServiceProgram program)
        {
            container.Received().Resolve <ISelkieLogger>();
        }

        [Theory]
        [AutoNSubstituteData]
        public void ConstructorCreatesSelkieConsoleTest([NotNull] [Frozen] IWindsorContainer container,
                                                        [NotNull] ServiceProgram program)
        {
            container.Received().Resolve <IServiceConsole>();
        }

        [Theory]
        [AutoNSubstituteData]
        public void LoggerIsNotNullTest([NotNull] ServiceProgram program)
        {
            Assert.NotNull(program.Logger);
        }

        [Theory]
        [AutoNSubstituteData]
        public void MainCallsReleaseForLoggerTest([NotNull] [Frozen] IWindsorContainer container,
                                                  [NotNull] ServiceProgram program)
        {
            // assemble
            // act
            program.Main(true);

            // assert
            container.Received().Release(Arg.Any <ISelkieLogger>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void MainCallsReleaseForServiceConsoleTest([NotNull] [Frozen] IWindsorContainer container,
                                                          [NotNull] ServiceProgram program)
        {
            // assemble
            // act
            program.Main(true);

            // assert
            container.Received().Release(Arg.Any <IServiceConsole>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void MainCallsStartOnServiceConsoleForIsWaitForKeyIsFalseTest([NotNull] IWindsorContainer container,
                                                                             [NotNull] IWindsorInstaller installer,
                                                                             [NotNull] IServiceConsole serviceConsole)
        {
            // assemble
            container.Resolve <IServiceConsole>().Returns(serviceConsole);

            var program = new ServiceProgram(container,
                                             installer);

            // act
            program.Main(false);

            // assert
            serviceConsole.Received().Start(false);
        }

        [Theory]
        [AutoNSubstituteData]
        public void MainCallsStartOnServiceConsoleTest([NotNull] IWindsorContainer container,
                                                       [NotNull] IWindsorInstaller installer,
                                                       [NotNull] IServiceConsole serviceConsole)
        {
            // assemble
            container.Resolve <IServiceConsole>().Returns(serviceConsole);

            var program = new ServiceProgram(container,
                                             installer);

            // act
            program.Main(true);

            // assert
            serviceConsole.Received().Start(true);
        }
    }
}