using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
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
                var highlighting = new MethodTooLong.Highlighting(Warnings.Warning_TooManyDeclarations);
                consumer.AddHighlighting(highlighting, constructorDeclaration.GetNameDocumentRange());
            }
        }

        protected override int Value
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDeclarationsMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDeclarationsEnabled); }
        }
    }
}