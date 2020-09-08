using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features
{
    public static class ExtensionMethodsCsharp
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

        public static IEnumerable<T> GetFlattenedHierarchyOfType<T>(this ITreeNode root) where T : class, ITreeNode
        {
            var list = new List<T>();
            if (root is T rootAsType)
                list.Add(rootAsType);

            list.AddRange(root.GetChildrenRecursive<T>());

            return list;
        }

        public static IEnumerable<T> GetChildrenRecursive<T>(this ITreeNode node) where T : ITreeNode
        {
            var nodeChildren = node.Children().ToList();

            var list = new List<T>();

            var childOfType = nodeChildren.OfType<T>();
            list.AddRange(childOfType);

            foreach (var childNode in nodeChildren)
            {
                var childrenOfType = GetChildrenRecursive<T>(childNode);
                list.AddRange(childrenOfType);
            }

            return list;
        }

        private static bool IsNodeThatIncreasesDepth(ITreeNode node)
        {
            switch (node)
            {
                case IIfStatement _:
                    return true;
                case IForeachStatement _:
                    return true;
                case IForStatement _:
                    return true;
                case ISwitchStatement _:
                    return true;
            }

            return false;
        }

        public static IType TryGetClosedReturnTypeFrom(ITreeNode treeNode)
        {
            switch (treeNode)
            {
                case IReferenceExpression reference:
                    return TryGetClosedReturnTypeFromReference(reference.Reference);
                case IInvocationExpression invocationExpression:
                    return TryGetClosedReturnTypeFromReference(invocationExpression.Reference);
                default: return null;
            }
        }

        public static IReferenceExpression TryGetFirstReferenceExpression(ITreeNode currentNode)
        {
            var childNodes = currentNode.Children();
            var firstChildNode = childNodes.FirstOrDefault();

            if (firstChildNode == null)
                return null;

            return firstChildNode as IReferenceExpression ??
                   TryGetFirstReferenceExpression(firstChildNode);
        }

        private static IType TryGetClosedReturnTypeFromReference(IReference reference)
        {
            var resolveResultWithInfo = GetResolveResult(reference);
            var declaredElement = resolveResultWithInfo.DeclaredElement;

            if (declaredElement is IParametersOwner parametersOwner)
            {
                var returnType = parametersOwner.ReturnType;
                return returnType.IsOpenType ? GetClosedType(resolveResultWithInfo, returnType) : returnType;
            }

            return null;
        }

        private static IType GetClosedType(ResolveResultWithInfo resolveResultWithInfo, IType returnType)
        {
            return resolveResultWithInfo.Result.Substitution.Apply(returnType);
        }

        public static ResolveResultWithInfo GetResolveResult(this IReference reference)
        {
            return reference.Resolve();
        }
    }
}