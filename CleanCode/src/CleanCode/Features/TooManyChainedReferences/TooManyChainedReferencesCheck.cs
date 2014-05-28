using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.TooManyChainedReferences
{
    public class TooManyChainedReferencesCheck : SimpleCheck<IReferenceExpression, int>
    {
        public TooManyChainedReferencesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IReferenceExpression methodDeclaration, IHighlightingConsumer consumer)
        {
            if (methodDeclaration != null && !ParentIsReference(methodDeclaration))
            {
                ProcessReference(methodDeclaration, consumer);
            }
        }

        private void ProcessReference(IReferenceExpression reference, IHighlightingConsumer consumer)
        {
            var length = reference.CountChildren<IReferenceExpression>();
            var maximumChainedReferences = Threshold;

            if (length > maximumChainedReferences)
            {
                var highlighting = new Highlighting(Warnings.ChainedReferences);
                consumer.AddHighlighting(highlighting, reference.GetDocumentRange());
            }
        }

        private static bool ParentIsReference(ITreeNode element)
        {
            var reference = element.Parent as IReferenceExpression;
            return reference != null;
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyChainedReferencesMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyChainedReferencesEnabled); }
        }
    }
}