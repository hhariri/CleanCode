using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
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
                var highlighting = new Highlighting(Warnings.ClassTooBig);
                consumer.AddHighlighting(highlighting, documentRange);
            }
        }

        protected override int Value
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.ClassTooBigMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.ClassTooBigEnabled); }
        }
    }
}