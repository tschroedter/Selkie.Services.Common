using System.Threading;
using Selkie.Windsor;

namespace Selkie.Services.Common
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