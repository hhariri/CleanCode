using System;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features
{
    public static class ExtensionMethods
    {
        public static int CountChildren<T>(this ITreeNode node) where T : ITreeNode
        {
            var treeNodes = node.Children().ToList();

            var childOfType = treeNodes.OfType<T>();
            var count = childOfType.Count();

            foreach (var childNodes in treeNodes)
            {
                count += CountChildren<T>(childNodes);
            }
            return count;
        }


        public static int GetChildrenDepth(this ITreeNode node)
        {           
            var childrenDepth = 0;
            var children = node.Children();
            foreach (var block in children)
            {
                var levelOfCurrentBlock = GetChildrenDepth(block);
                childrenDepth = Math.Max(levelOfCurrentBlock, childrenDepth);
            }

            if (IsNodeThatIncreasesDepth(node))
            {
                return childrenDepth + 1;    
            }
            return childrenDepth;
        }

        private static bool IsNodeThatIncreasesDepth(ITreeNode node)
        {
            if (node is IIfStatement)
            {
                return true;
            }
            if (node is IForeachStatement)
            {
                return true;
            }
            if (node is IForStatement)
            {
                return true;
            }
            if (node is ISwitchStatement)
            {
                return true;
            }

            return false;
        }

        public static IType TryGetClosedReturnTypeFrom(ITreeNode treeNode)
        {
            IType type = null;
            var reference = treeNode as IReferenceExpression;
            if (reference != null)
            {
                type = TryGetClosedReturnTypeFromReference(reference.Reference);
            }

            var invocationExpression = treeNode as IInvocationExpression;
            if (invocationExpression != null)
            {
                type = TryGetClosedReturnTypeFromReference(invocationExpression.Reference);
            }

            return type;
        }

        public static IReferenceExpression TryGetFirstReferenceExpression(ITreeNode currentNode)
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

        private static IType TryGetClosedReturnTypeFromReference(IReference reference)
        {
            var resolveResultWithInfo = GetResolveResult(reference);

            if (reference.CurrentResolveResult == null)
            {
                reference.Resolve();
            }

            var declaredElement = resolveResultWithInfo.DeclaredElement;
            var parametersOwner = declaredElement as IParametersOwner;

            if (parametersOwner != null)
            {
                var returnType = parametersOwner.ReturnType;
                if (returnType.IsOpenType)
                {
                    return GetClosedType(resolveResultWithInfo, returnType);
                }

                return returnType;
            }

            return null;
        }

        private static IType GetClosedType(ResolveResultWithInfo resolveResultWithInfo, IType returnType)
        {
            var closedType = resolveResultWithInfo.Result.Substitution.Apply(returnType);
            return closedType;
        }

        private static ResolveResultWithInfo GetResolveResult(IReference reference)
        {
            if (reference.CurrentResolveResult != null)
            {
                return reference.CurrentResolveResult;
            }

            return reference.Resolve();
        }
    }
}