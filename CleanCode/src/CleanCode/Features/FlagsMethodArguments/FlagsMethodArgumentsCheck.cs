using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using System.Linq;

namespace CleanCode.Features.FlagsMethodArguments
{
    public class FlagsMethodArgumentsCheck : SimpleCheck<IMethodDeclaration, int>
    {
        public FlagsMethodArgumentsCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration methodDeclaration, IHighlightingConsumer consumer)
        {
            var threshold = Threshold;

            var numberOfFlagsArgument = methodDeclaration.ParameterDeclarations.Count(
                    declaration => declaration.Type.ToString() == typeof(System.Boolean).FullName);

            if (numberOfFlagsArgument > threshold)
            {
                var highlighting = new Highlighting(Warnings.FlagMethodArguments);
                consumer.AddHighlighting(highlighting, methodDeclaration.GetNameDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.FlagMethodArgumentsMinimum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.FlagMethodArgumentsEnabled); }
        }
    }
}