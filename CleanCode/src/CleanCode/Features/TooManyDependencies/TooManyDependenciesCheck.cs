using System.Linq;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

namespace CleanCode.Features.TooManyDependencies
{
    public class TooManyDependenciesCheck : SimpleCheck<IConstructorDeclaration, int>
    {
        public TooManyDependenciesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IConstructorDeclaration statement, IHighlightingConsumer consumer)
        {
            var maxDependencies = Threshold;

            var depedencies = statement.ParameterDeclarations.Select(
                declaration => declaration.DeclaredElement != null &&
                               declaration.DeclaredElement.Type.IsInterfaceType());

            var dependenciesCount = depedencies.Count();

            if (dependenciesCount > maxDependencies)
            {
                var highlighting = new Highlighting(Warnings.TooManyDependencies);
                consumer.AddHighlighting(highlighting, statement.GetNameDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDependenciesMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDependenciesMaximumEnabled); }
        }
    }
}