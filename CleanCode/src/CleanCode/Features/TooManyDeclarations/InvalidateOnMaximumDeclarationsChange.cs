using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode.Features.TooManyDeclarations
{
    [SolutionComponent]
    public class InvalidateOnMaximumDeclarationsChange
    {
        public InvalidateOnMaximumDeclarationsChange(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
        {
            var maxLines = settingsStore.Schema.GetScalarEntry((CleanCodeSettings s) => s.TooManyDeclarationsMaximum);
            settingsStore.AdviseChange(lifetime, maxLines, daemon.Invalidate);
        }
    }
}