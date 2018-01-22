using System.Runtime.CompilerServices;
using Core2.Selkie.Common;
using JetBrains.Annotations;

[assembly : InternalsVisibleTo("Core2.Selkie.Services.Common.Tests")]

namespace Core2.Selkie.Services.Common
{
    [UsedImplicitly]
    public class Installer : SelkieInstaller <Installer>
    {
    }
}