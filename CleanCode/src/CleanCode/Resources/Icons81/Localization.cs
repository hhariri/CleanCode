using JetBrains.Annotations;

namespace CleanCode.Resources.Icons81
{

    public class LocalizedStrings
    {
        [CanBeNull]
        private static readonly Common LocalizedResourcesMember = new Common();

        internal Common LocalizedResources { get { return LocalizedResourcesMember; } }
    }
}