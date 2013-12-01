using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
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
            return GetChildrenDepth(node, 0);
        }

        private static int GetChildrenDepth(this ITreeNode node, int myLevel)
        {
            var maxLevel = myLevel;

            var blocks = GetChildrenThatIncreaseDepth(node);

            int maxLevelOfChildren = 0;
            foreach (var block in blocks)
            {
                var levelOfCurrentBlock = GetChildrenDepth(block, myLevel + 1);
                maxLevelOfChildren = Math.Max(levelOfCurrentBlock, maxLevelOfChildren);
            }

            return 1 + maxLevelOfChildren;
        }

        private static IEnumerable<ITreeNode> GetChildrenThatIncreaseDepth(ITreeNode node)
        {
            var navigableNodes = new List<ITreeNode>();

            var blocks = node.Children<IBlock>();
            var loops = node.Children<ILoopStatement>();
            var ifStatements = node.Children<IIfStatement>();
            var switches = node.Children<ISwitchStatement>();

            navigableNodes.AddRange(blocks);
            navigableNodes.AddRange(loops);
            navigableNodes.AddRange(ifStatements);
            navigableNodes.AddRange(switches);

            return navigableNodes;
        }
    }
}