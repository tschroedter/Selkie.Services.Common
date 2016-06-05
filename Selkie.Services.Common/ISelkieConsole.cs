using JetBrains.Annotations;

namespace Selkie.Services.Common
{
    public interface ISelkieConsole
    {
        [NotNull]
        string ReadLine();

        void WriteLine([NotNull] string text);
    }
}