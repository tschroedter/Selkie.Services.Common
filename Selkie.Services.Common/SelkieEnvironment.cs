using System;
using System.Diagnostics.CodeAnalysis;
using Selkie.Windsor;

namespace Selkie.Services.Common
{
    [ProjectComponent(Lifestyle.Transient)]
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class SelkieEnvironment : ISelkieEnvironment
    {
        public void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }

    public interface ISelkieEnvironment
    {
        void Exit(int exitCode);
    }
}