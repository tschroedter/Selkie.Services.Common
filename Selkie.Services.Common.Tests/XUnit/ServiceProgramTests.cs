using System.Diagnostics.CodeAnalysis;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Common.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class ServiceProgramTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void ConstructorCallsInstallTest([NotNull] [Frozen] IWindsorContainer container,
                                                [NotNull] [Frozen] IWindsorInstaller installer,
                                                [NotNull] ServiceProgram program)
        {
            container.Received()
                     .Install(installer);
        }

        [Theory]
        [AutoNSubstituteData]
        public void ConstructorCreatesLoggerTest([NotNull] [Frozen] IWindsorContainer container,
                                                 [NotNull] ServiceProgram program)
        {
            container.Received()
                     .Resolve <ILogger>();
        }

        [Theory]
        [AutoNSubstituteData]
        public void ConstructorCreatesSelkieConsoleTest([NotNull] [Frozen] IWindsorContainer container,
                                                        [NotNull] ServiceProgram program)
        {
            container.Received()
                     .Resolve <IServiceConsole>();
        }

        [Theory]
        [AutoNSubstituteData]
        public void LoggerIsNotNullTest([NotNull] ServiceProgram program)
        {
            Assert.NotNull(program.Logger);
        }

        [Theory]
        [AutoNSubstituteData]
        public void MainCallsStartOnServiceConsoleTest([NotNull] IWindsorContainer container,
                                                       [NotNull] IWindsorInstaller installer,
                                                       [NotNull] IServiceConsole serviceConsole)
        {
            // assemble
            container.Resolve <IServiceConsole>()
                     .Returns(serviceConsole);

            ServiceProgram program = new ServiceProgram(container,
                                                        installer);

            // act
            program.Main(true);

            // assert
            serviceConsole.Received()
                          .Start(true);
        }

        [Theory]
        [AutoNSubstituteData]
        public void MainCallsStartOnServiceConsoleForIsWaitForKeyIsFalseTest([NotNull] IWindsorContainer container,
                                                                             [NotNull] IWindsorInstaller installer,
                                                                             [NotNull] IServiceConsole serviceConsole)
        {
            // assemble
            container.Resolve <IServiceConsole>()
                     .Returns(serviceConsole);

            ServiceProgram program = new ServiceProgram(container,
                                                        installer);

            // act
            program.Main(false);

            // assert
            serviceConsole.Received()
                          .Start(false);
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
            container.Received()
                     .Release(Arg.Any <ILogger>());
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
            container.Received()
                     .Release(Arg.Any <IServiceConsole>());
        }
    }
}