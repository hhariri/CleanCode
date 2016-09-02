using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode.Features.TooManyDependencies
{
  [SolutionComponent]
  public class InvalidateOnMaximumDependenciesChange
  {
    public InvalidateOnMaximumDependenciesChange(Lifetime lifetime, IDaemon daemon, ISettingsStore settingsStore)
    {
      var maxParams = settingsStore.Schema.GetScalarEntry((CleanCodeSettings s) => s.TooManyDependenciesMaximum);
      settingsStore.AdviseChange(lifetime, maxParams, daemon.Invalidate);
    }
  }
}