using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode.Features.MethodNameNotMeaningful
{
    [SolutionComponent]
    public class InvalidateOnMinimumMethodNameLenghtChange
    {
        public InvalidateOnMinimumMethodNameLenghtChange(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
        {
            var minMethodNameLenght = settingsStore.Schema.GetScalarEntry((CleanCodeSettings s) => s.MethodNameNotMeaningfulMinimumEnabled);
            settingsStore.AdviseChange(lifetime, minMethodNameLenght, daemon.Invalidate);
        }
    }
}