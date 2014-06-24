using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
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

        protected override void ExecuteCore(IMethodDeclaration classDeclaration, IHighlightingConsumer consumer)
        {
            var parameterDeclarations = classDeclaration.ParameterDeclarations;
            var maxParameters = Value;

            if (parameterDeclarations.Count > maxParameters)
            {
                var highlighting = new Highlighting(Warnings.TooManyMethodArguments);
                consumer.AddHighlighting(highlighting, classDeclaration.GetNameDocumentRange());
            }
        }

        protected override int Value
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyMethodArgumentsMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyMethodArgumentsEnabled); }
        }
    }
}