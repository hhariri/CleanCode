using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.MethodNameNotMeaningful
{
    public class MethodNamesNotMeaningfulCheck : SimpleCheck<IMethodDeclaration, int>
    {
        public MethodNamesNotMeaningfulCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration methodDeclaration, IHighlightingConsumer consumer)
        {
            var minimumMethodNameLenght = Threshold;

            if (methodDeclaration.NameIdentifier == null)
            {
                return;
            }

            var name = methodDeclaration.NameIdentifier.GetText();
            var methodNameLenght = name.Length;
            if (methodNameLenght < minimumMethodNameLenght)
            {
                var highlighting = new Highlighting(Warnings.MethodNameNotMeaningful);
                consumer.AddHighlighting(highlighting, methodDeclaration.GetNameDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.MethodNameNotMeaningfulMinimum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.MethodNameNotMeaningfulMinimumEnabled); }
        }
    }
}