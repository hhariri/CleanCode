using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode
{
    [SolutionComponent]
    public class InvalidateOnSettingsChange
    {
        public InvalidateOnSettingsChange(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
        {
            var settingsKey = settingsStore.Schema.GetKey<CleanCodeSettings>();
            settingsStore.AdviseChange(lifetime, settingsKey, daemon.Invalidate);
        }
    }
}