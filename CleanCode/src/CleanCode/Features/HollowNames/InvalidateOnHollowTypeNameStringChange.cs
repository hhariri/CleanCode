using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode.Features.HollowNames
{    

    [SolutionComponent]
    public class InvalidateOnHollowTypeNameStringChange
    {
        public InvalidateOnHollowTypeNameStringChange(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
        {
            var maxLines = settingsStore.Schema.GetScalarEntry((CleanCodeSettings s) => s.HollowTypeNameString);
            settingsStore.AdviseChange(lifetime, maxLines, daemon.Invalidate);
        }
    }
}