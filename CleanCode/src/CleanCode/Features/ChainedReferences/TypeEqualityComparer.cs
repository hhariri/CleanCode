using System.Collections.Generic;
using JetBrains.ReSharper.Psi;

namespace CleanCode.Features.ChainedReferences
{
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