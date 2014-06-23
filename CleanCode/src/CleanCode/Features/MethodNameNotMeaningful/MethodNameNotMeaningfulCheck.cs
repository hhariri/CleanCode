using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.MethodNameNotMeaningful
{
    public class MethodNameNotMeaningfulCheck : SimpleCheck<IMethodDeclaration, int>
    {
        public MethodNameNotMeaningfulCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration statement, IHighlightingConsumer consumer)
        {
            var minimumMethodNameLenght = Threshold;

            if (statement.NameIdentifier == null)
            {
                return;
            }

            var name = statement.NameIdentifier.GetText();
            var methodNameLenght = name.Length;
            if (methodNameLenght < minimumMethodNameLenght)
            {
                var highlighting = new Highlighting(Warnings.MethodNameNotMeaningful);
                consumer.AddHighlighting(highlighting, statement.GetNameDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.MethodNameNotMeaningfulMinimum); }
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.MethodNameNotMeaningfulMinimumEnabled); }
        }
    }
}