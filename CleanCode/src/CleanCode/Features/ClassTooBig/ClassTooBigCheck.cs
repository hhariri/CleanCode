using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ClassTooBig
{
    public class ClassTooBigCheck : MonoValueCheck<IClassDeclaration, int>
    {
        public ClassTooBigCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IClassDeclaration constructorDeclaration, IHighlightingConsumer consumer)
        {
            var maxLength = Value;

            var statementCount = constructorDeclaration.CountChildren<IMethodDeclaration>();
            if (statementCount > maxLength)
            {
                var declarationIdentifier = constructorDeclaration.NameIdentifier;
                var documentRange = declarationIdentifier.GetDocumentRange();
                var highlighting = new Highlighting(Warnings.ClassTooBig, documentRange);
                consumer.AddHighlighting(highlighting);
            }
        }

        protected override int Value
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ClassTooBigMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ClassTooBigEnabled); }
        }
    }
}