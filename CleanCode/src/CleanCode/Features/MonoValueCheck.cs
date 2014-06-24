using JetBrains.Application.Settings;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features
{
    public abstract class MonoValueCheck<TElement, TValue> : Check<TElement>
        where TElement : ITreeNode
    {
        protected MonoValueCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected abstract TValue Value { get; }
    }
}