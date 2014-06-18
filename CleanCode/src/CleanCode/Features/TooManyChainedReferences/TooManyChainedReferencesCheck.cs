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
    using JetBrains;
    using JetBrains.ReSharper.Psi.Resolve;

    public class TooManyChainedReferencesCheck : SimpleCheck<IStatement, int>
    {
        public TooManyChainedReferencesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IStatement referenceExpression, IHighlightingConsumer consumer)
        {
            if (referenceExpression != null)
            {
                this.HighlightMethodChainsThatAreTooLong(referenceExpression, consumer);
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

            ITreeNode child = referenceExpression;
            int chainedCount = 0;
            
            while (child != null)
            {
                var childReturnType = this.GetReturnTypeFrom(child);

                if (childReturnType != null)
                {
                    types.Add(childReturnType);
                }

                child = GetFirstChild(child);
                chainedCount++;
            }

            var isFluentChain = types.Count == 1;

            if (!isFluentChain)
            {
                if (chainedCount > Threshold)
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

        private static ITreeNode GetFirstChild(ITreeNode referenceExpression)
        {
            return referenceExpression.Children().FirstOrDefault();
        }

        private IList<ITreeNode> GetChainedNodes(ITreeNode statement)
        {
            var list = new List<ITreeNode>();
            var firstOrDefault = statement.Children().FirstOrDefault();
            if (firstOrDefault != null)
            {
                list.Add(firstOrDefault);
                list.AddRange(this.GetChainedNodes(firstOrDefault));
            }
            return list;
        }

        private static IType GetRootType(IReferenceExpression reference)
        {
            var referenceOfReference = reference.Reference;
            referenceOfReference.Resolve();

            var rootType = GetReturnTypeFromReference(referenceOfReference);
            return rootType;
        }

        private bool RootTypeAndChainedReferencesTypesAreDifferent(IReferenceExpression reference, IType rootType)
        {
            var childrenTypes = this.GetTypesFromChildren(reference);
            return SomeTypeIsDifferent(rootType, childrenTypes);
        }

        private IEnumerable<IType> GetTypesFromChildren(IReferenceExpression reference)
        {
            var children = reference.GetChildrenRecursive<IReferenceExpression>().Where(expression => expression.Reference.IsQualified);
            var typesFromChildren = children.Select(expression => GetReturnTypeFromReference(expression.Reference));
            return typesFromChildren;
        }

        private static void AddHighlightning(IReferenceExpression reference, IHighlightingConsumer consumer)
        {
            var highlighting = new Highlighting(Warnings.ChainedReferences);
            var nameIdentifier = reference.NameIdentifier;
            consumer.AddHighlighting(highlighting, nameIdentifier.GetDocumentRange());
        }

        private static bool SomeTypeIsDifferent(IType type, IEnumerable<IType> typesFromChildren)
        {
            bool someTypeIsDifferent = typesFromChildren.Any(otherType => Equals(type, otherType));
            return someTypeIsDifferent;
        }

        private static bool Equals(IType type, IType otherType)
        {
            if (type == null || otherType == null)
            {
                return false;
            }

            return type.ToString().Equals(otherType.ToString());
        }

        private static bool IsTypeStillUnknown(IType type)
        {
            return type == null;
        }

        private static IType GetReturnTypeFromReference(IReference reference)
        {
            reference.Resolve();

            if (reference.CurrentResolveResult != null)
            {
                var declaredElement = reference.CurrentResolveResult.DeclaredElement;
                var parameternsOwner = declaredElement as IParametersOwner;
                if (parameternsOwner != null)
                {
                    return parameternsOwner.ReturnType;
                }
            }

            return null;
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

    internal class TypeEqualityComparer : IEqualityComparer<IType>
    {
        public bool Equals(IType x, IType y)
        {
            return x.ToString().Equals(y.ToString());
        }

        public int GetHashCode(IType obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}