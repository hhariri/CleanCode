using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.TooManyMethodArguments
{
    public class TooManyMethodArgumentsCheck : MonoValueCheck<IMethodDeclaration, int>
    {
        public TooManyMethodArgumentsCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration constructorDeclaration, IHighlightingConsumer consumer)
        {
            var parameterDeclarations = constructorDeclaration.ParameterDeclarations;
            var maxParameters = Value;

            if (parameterDeclarations.Count > maxParameters)
            {
                var highlighting = new Highlighting(Warnings.TooManyMethodArguments,
                    constructorDeclaration.GetNameDocumentRange());
                consumer.AddHighlighting(highlighting);
            }
        }

        protected override int Value
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyMethodArgumentsMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyMethodArgumentsEnabled); }
        }
    }
}