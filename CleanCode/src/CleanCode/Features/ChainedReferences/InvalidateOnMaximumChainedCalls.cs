using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode.Features.ChainedReferences
{
    [SolutionComponent]
    public class InvalidateOnMaximumChainedCalls
    {
        public InvalidateOnMaximumChainedCalls(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
        {
            var maxDepth = settingsStore.Schema.GetScalarEntry((CleanCodeSettings s) => s.TooManyChainedReferencesMaximum);
            settingsStore.AdviseChange(lifetime, maxDepth, daemon.Invalidate);
        }
    }
}