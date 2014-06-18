using System.Diagnostics;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;


namespace CleanCode.Features.TooManyChainedReferences
{
    using JetBrains.ReSharper.Psi.Resolve;

    public class TooManyChainedReferencesCheck : SimpleCheck<ICSharpStatement, int>
    {
        public TooManyChainedReferencesCheck(IContextBoundSettingsStore settingsStore)
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
            var equalityComparer = new TypeEqualityComparer();
            var types = new HashSet<IType>(equalityComparer);

            var nextReferenceExpression = referenceExpression;
            var chainLength = 0;

            while (nextReferenceExpression != null)
            {
                var childReturnType = this.GetReturnTypeFrom(nextReferenceExpression);

                if (childReturnType != null)
                {
                    types.Add(childReturnType);
                    chainLength++;
                }

                nextReferenceExpression = TryGetFirstReferenceExpression(nextReferenceExpression);
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

        private IType GetReturnTypeFrom(ITreeNode treeNode)
        {
            IType type = null;
            var reference = treeNode as IReferenceExpression;
            if (reference != null)
            {
                type = GetReturnTypeFromReference(reference.Reference);
            }

            var invocationExpression = treeNode as IInvocationExpression;
            if (invocationExpression != null)
            {
                type = GetReturnTypeFromReference(invocationExpression.Reference);
            }

            return type;
        }

        private static IReferenceExpression TryGetFirstReferenceExpression(ITreeNode currentNode)
        {
            var childNodes = currentNode.Children();
            var firstChildNode = childNodes.FirstOrDefault();

            if (firstChildNode == null)
            {
                return null;
            }
            else
            {
                var referenceExpression = firstChildNode as IReferenceExpression;

                if (referenceExpression == null)
                {
                    referenceExpression = TryGetFirstReferenceExpression(firstChildNode);
                }

                return referenceExpression;    
            }            
        }

        private static void AddHighlightning(IReferenceExpression reference, IHighlightingConsumer consumer)
        {
            var highlighting = new Highlighting(Warnings.ChainedReferences);
            var nameIdentifier = reference.NameIdentifier;
            consumer.AddHighlighting(highlighting, nameIdentifier.GetDocumentRange());
        }

        private static IType GetReturnTypeFromReference(IReference reference)
        {
            if (reference.CurrentResolveResult == null)
            {
                reference.Resolve();
            }

            Debug.Assert(reference.CurrentResolveResult != null, "reference.CurrentResolveResult != null");

            var declaredElement = reference.CurrentResolveResult.DeclaredElement;
            var parameternsOwner = declaredElement as IParametersOwner;

            return parameternsOwner != null ? parameternsOwner.ReturnType : null;
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