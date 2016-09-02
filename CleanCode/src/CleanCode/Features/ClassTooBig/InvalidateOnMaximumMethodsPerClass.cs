using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode.Features.ClassTooBig
{
    [SolutionComponent]
    public class InvalidateOnMaximumMethodsPerClass
    {
        public InvalidateOnMaximumMethodsPerClass(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
        {
            var maxDepth = settingsStore.Schema.GetScalarEntry((CleanCodeSettings s) => s.ClassTooBigMaximum);
            settingsStore.AdviseChange(lifetime, maxDepth, daemon.Invalidate);
        }
    }
}