using System.Collections.Generic;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ChainedReferences
{
    public class ChainedReferencesCheck : MonoValueCheck<ICSharpStatement, int>
    {
        public ChainedReferencesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(ICSharpStatement constructorDeclaration, IHighlightingConsumer consumer)
        {
            if (constructorDeclaration != null && !constructorDeclaration.IsEmbeddedStatement)
            {
                HighlightMethodChainsThatAreTooLong(constructorDeclaration, consumer);
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
                    HightlightReferenceExpressionIfNeeded(referenceExpression, consumer);
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
                if (chainLength > Value)
                {
                    AddHighlightning(referenceExpression, consumer);
                }
            }
        }

        protected override int Value
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyChainedReferencesMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyChainedReferencesEnabled); }
        }

        private static void AddHighlightning(IReferenceExpression reference, IHighlightingConsumer consumer)
        {
            var nameIdentifier = reference.NameIdentifier;
            var documentRange = nameIdentifier.GetDocumentRange();
            var highlighting = new Highlighting(Warnings.ChainedReferences, documentRange);
            consumer.AddHighlighting(highlighting);
        }
    }
}