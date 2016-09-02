using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode.Features.ComplexExpression
{
    [SolutionComponent]
    public class InvalidateOnComplexExpressionChange
    {
        public InvalidateOnComplexExpressionChange(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
        {
            var maxDepth = settingsStore.Schema.GetScalarEntry((CleanCodeSettings s) => s.ComplexExpressionMaximum);
            settingsStore.AdviseChange(lifetime, maxDepth, daemon.Invalidate);
        }
    }
}