using JetBrains.Annotations;

namespace Core2.Selkie.Services.Common.Interfaces
{
    public interface ISelkieConsole
    {
        [NotNull]
        [UsedImplicitly]
        string ReadLine();

        void WriteLine([NotNull] string text);
    }
}