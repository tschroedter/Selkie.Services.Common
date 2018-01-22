using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common
{
    public interface ISelkieConsole
    {
        [NotNull]
        string ReadLine();

        void WriteLine([NotNull] string text);
    }
}