using System.Threading;
using Core2.Selkie.Windsor;

namespace Core2.Selkie.Services.Common
{
    [ProjectComponent(Lifestyle.Transient)]
    public class SelkieSleeper : ISelkieSleeper
    {
        public void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }
    }
}