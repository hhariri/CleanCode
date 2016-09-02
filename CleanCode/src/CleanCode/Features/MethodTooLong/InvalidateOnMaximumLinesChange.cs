using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode.Features.MethodTooLong
{
    [SolutionComponent]
    public class InvalidateOnMaximumLinesChange
    {
        public InvalidateOnMaximumLinesChange(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
        {
            var maxLines = settingsStore.Schema.GetScalarEntry((CleanCodeSettings s) => s.MethodTooLongMaximum);
            settingsStore.AdviseChange(lifetime, maxLines, daemon.Invalidate);
        }
    }
}