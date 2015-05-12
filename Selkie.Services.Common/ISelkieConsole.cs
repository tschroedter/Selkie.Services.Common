using JetBrains.Annotations;

namespace Selkie.Services.Common
{
    public interface ISelkieConsole
    {
        void WriteLine([NotNull] string text);

        [NotNull]
        string ReadLine();
    }
}