using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.MethodNameNotMeaningful
{
    public class MethodNameNotMeaningfulCheck : MonoValueCheck<IMethodDeclaration, int>
    {
        public MethodNameNotMeaningfulCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration constructorDeclaration, IHighlightingConsumer consumer)
        {
            var minimumMethodNameLenght = Value;

            if (constructorDeclaration.NameIdentifier == null)
            {
                return;
            }

            var name = constructorDeclaration.NameIdentifier.GetText();
            var methodNameLenght = name.Length;
            if (methodNameLenght < minimumMethodNameLenght)
            {
                var documentRange = constructorDeclaration.GetNameDocumentRange();
                var highlighting = new Highlighting(Warnings.MethodNameNotMeaningful, documentRange);
                consumer.AddHighlighting(highlighting);
            }
        }

        protected override int Value
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.MethodNameNotMeaningfulMinimum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.MethodNameNotMeaningfulMinimumEnabled); }
        }
    }
}