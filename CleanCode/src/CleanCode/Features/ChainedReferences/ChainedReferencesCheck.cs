using System.Collections.Generic;
using System.Linq;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ChainedReferences
{
    public class ChainedReferencesCheck : SimpleCheck<ICSharpStatement, int>
    {
        public ChainedReferencesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(ICSharpStatement statement, IHighlightingConsumer consumer)
        {
            if (statement != null && !statement.IsEmbeddedStatement)
            {
                this.HighlightMethodChainsThatAreTooLong(statement, consumer);
            }
        }

        private void HighlightMethodChainsThatAreTooLong(ITreeNode statement, IHighlightingConsumer consumer)
        {
            var children = statement.Children();

            foreach (var treeNode in children)
            {
                var referenceExpression = treeNode as IReferenceExpression;
                if (referenceExpression != null)
                {
                    this.HightlightReferenceExpressionIfNeeded(referenceExpression, consumer);
                }
                else
                {
                    HighlightMethodChainsThatAreTooLong(treeNode, consumer);
                }
            }
        }

        private void HightlightReferenceExpressionIfNeeded(IReferenceExpression referenceExpression, IHighlightingConsumer consumer)
        {
            var types = new HashSet<IType>();

            var nextReferenceExpression = referenceExpression;
            var chainLength = 0;

            while (nextReferenceExpression != null)
            {
                var childReturnType = ExtensionMethods.TryGetClosedReturnTypeFrom(nextReferenceExpression);

                if (childReturnType != null)
                {
                    types.Add(childReturnType);
                    chainLength++;
                }

                nextReferenceExpression = ExtensionMethods.TryGetFirstReferenceExpression(nextReferenceExpression);
            }

            var isFluentChain = types.Count == 1;

            if (!isFluentChain)
            {
                if (chainLength > Threshold)
                {
                    AddHighlightning(referenceExpression, consumer);
                }
            }
        }

        protected override int Threshold
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyChainedReferencesMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyChainedReferencesEnabled); }
        }

        public static void AddHighlightning(IReferenceExpression reference, IHighlightingConsumer consumer)
        {
            var highlighting = new Highlighting(Warnings.ChainedReferences);
            var nameIdentifier = reference.NameIdentifier;
            consumer.AddHighlighting(highlighting, nameIdentifier.GetDocumentRange());
        }
    }
}