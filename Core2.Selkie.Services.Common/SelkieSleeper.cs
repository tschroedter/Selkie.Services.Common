using System.Threading;
using Core2.Selkie.Services.Common.Interfaces;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common
{
    [ProjectComponent(Lifestyle.Transient)]
    [UsedImplicitly]
    public class SelkieSleeper : ISelkieSleeper
    {
        public void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }
    }
}