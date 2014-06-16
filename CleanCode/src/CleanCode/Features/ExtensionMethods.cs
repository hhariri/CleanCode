using System;
using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features
{
    using System.Collections.Generic;

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
    }
}