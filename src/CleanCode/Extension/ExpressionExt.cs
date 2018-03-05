using System.Linq;
using CleanCode.Features;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Extension
{
    public static class ExpressionExt
    {
        public static int GetExpressionCount<T>(this IExpression expression) where T : ITreeNode => expression.GetChildrenRecursive<T>().Count();
    }
}