using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode.Features.TooManyMethodArguments
{
    [SolutionComponent]
    public class InvalidateOnMaximumMethodArgumentsChange
    {
        public InvalidateOnMaximumMethodArgumentsChange(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
        {
            var maxArguments =
                settingsStore.Schema.GetScalarEntry((CleanCodeSettings s) => s.TooManyMethodArgumentsMaximum);
            settingsStore.AdviseChange(lifetime, maxArguments, daemon.Invalidate);
        }
    }
}