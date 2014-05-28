using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.TooManyMethodArguments
{
    public class TooManyMethodArgumentsCheck : SimpleCheck<IMethodDeclaration, int>
    {
        public TooManyMethodArgumentsCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration methodDeclaration, IHighlightingConsumer consumer)
        {
            var parameterDeclarations = methodDeclaration.ParameterDeclarations;
            var maxParameters = Threshold;

            if (parameterDeclarations.Count > maxParameters)
            {
                var highlighting = new Highlighting(Warnings.TooManyMethodArguments);
                consumer.AddHighlighting(highlighting, methodDeclaration.GetNameDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyMethodArgumentsMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyMethodArgumentsEnabled); }
        }
    }
}