using System;
using System.Diagnostics.CodeAnalysis;
using Core2.Selkie.Windsor;

namespace Core2.Selkie.Services.Common
{
    [ProjectComponent(Lifestyle.Transient)]
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class SelkieConsole : ISelkieConsole
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public string ReadLine()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return Console.ReadLine();
        }
    }
}