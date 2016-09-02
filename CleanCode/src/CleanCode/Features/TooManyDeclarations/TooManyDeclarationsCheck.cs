using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.TooManyDeclarations
{
    public class TooManyDeclarationsCheck : MonoValueCheck<IMethodDeclaration, int>
    {
        public TooManyDeclarationsCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration constructorDeclaration, IHighlightingConsumer consumer)
        {
            var maxLength = Value;

            var statementCount = constructorDeclaration.CountChildren<IDeclaration>();
            if (statementCount > maxLength)
            {
                var highlighting = new MethodTooLong.Highlighting(Warnings.Warning_TooManyDeclarations,
                    constructorDeclaration.GetNameDocumentRange());
                consumer.AddHighlighting(highlighting);
            }
        }

        protected override int Value
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDeclarationsMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDeclarationsEnabled); }
        }
    }
}