using System.Linq;
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
    }
}