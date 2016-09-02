using System.Linq;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

namespace CleanCode.Features.TooManyDependencies
{
    public class TooManyDependenciesCheck : MonoValueCheck<IConstructorDeclaration, int>
    {
        public TooManyDependenciesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IConstructorDeclaration constructorDeclaration, IHighlightingConsumer consumer)
        {
            var maxDependencies = Value;

            var dependencies = constructorDeclaration.ParameterDeclarations.Select(
                declaration => declaration.DeclaredElement != null &&
                               declaration.DeclaredElement.Type.IsInterfaceType());

            var dependenciesCount = dependencies.Count();

            if (dependenciesCount > maxDependencies)
            {
                var highlighting = new Highlighting(Warnings.TooManyDependencies,
                    constructorDeclaration.GetNameDocumentRange());
                consumer.AddHighlighting(highlighting);
            }
        }

        protected override int Value
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDependenciesMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDependenciesMaximumEnabled); }
        }
    }
}