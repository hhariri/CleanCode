using System.Linq;
using JetBrains.ReSharper.Psi.VB.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features
{
    public static class ExtensionMethodsVb
    {
        public static IReferenceExpression TryGetFirstReferenceExpression(ITreeNode currentNode)
        {
            var childNodes = currentNode.Children();
            var firstChildNode = childNodes.FirstOrDefault();

            if (firstChildNode == null)
                return null;

            return firstChildNode as IReferenceExpression ??
                   TryGetFirstReferenceExpression(firstChildNode);
        }
    }
}