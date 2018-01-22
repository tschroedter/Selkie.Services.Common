using System;
using System.Diagnostics.CodeAnalysis;
using Core2.Selkie.Services.Common.Interfaces;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common
{
    [ProjectComponent(Lifestyle.Transient)]
    [ExcludeFromCodeCoverage]
    [UsedImplicitly]
    public class SelkieConsole : ISelkieConsole
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public string ReadLine()
        {
            string readLine = Console.ReadLine();

            if ( readLine == null )
            {
                throw new Exception("Can't read line from Console!");
            }

            return readLine;
        }
    }
}