using System.Collections.Generic;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ChainedReferences
{
    [ElementProblemAnalyzer(typeof(ICSharpStatement), HighlightingTypes = new []
    {
        typeof(MaximumChainedReferencesHighlighting)
    })]
    public class ChainedReferencesCheck : ElementProblemAnalyzer<ICSharpStatement>
    {
        protected override void Run(ICSharpStatement element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!element.CanBeEmbedded)
            {
                var threshold = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumChainedReferences);
                HighlightMethodChainsThatAreTooLong(element, consumer, threshold);
            }
        }

        private void HighlightMethodChainsThatAreTooLong(ITreeNode statement, IHighlightingConsumer consumer, int threshold)
        {
            var children = statement.Children();

            foreach (var treeNode in children)
            {
                var referenceExpression = treeNode as IReferenceExpression;
                if (referenceExpression != null)
                {
                    HighlightReferenceExpressionIfNeeded(referenceExpression, consumer, threshold);
                }
                else
                {
                    HighlightMethodChainsThatAreTooLong(treeNode, consumer, threshold);
                }
            }
        }

        private void HighlightReferenceExpressionIfNeeded(IReferenceExpression referenceExpression, IHighlightingConsumer consumer, int threshold)
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
                if (chainLength > threshold)
                {
                    AddHighlighting(referenceExpression, consumer);
                }
            }
        }

        private static void AddHighlighting(IReferenceExpression reference, IHighlightingConsumer consumer)
        {
            var nameIdentifier = reference.NameIdentifier;
            var documentRange = nameIdentifier.GetDocumentRange();
            var highlighting = new MaximumChainedReferencesHighlighting(Warnings.ChainedReferences, documentRange);
            consumer.AddHighlighting(highlighting);
        }
    }
}