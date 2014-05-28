using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features
{
    public abstract class SimpleCheck<TElement, TThreshold> where TElement : ITreeNode
    {
        private readonly IContextBoundSettingsStore settingsStore;

        protected SimpleCheck(IContextBoundSettingsStore settingsStore)
        {
            this.settingsStore = settingsStore;
        }

        public void ExecuteIfEnabled(TElement methodDeclaration, IHighlightingConsumer context)
        {
            if (!IsEnabled)
            {
                return;
            }
            ExecuteCore(methodDeclaration, context);
        }

        protected abstract void ExecuteCore(TElement element, IHighlightingConsumer consumer);

        protected abstract TThreshold Threshold { get; }

        protected abstract bool IsEnabled { get; }

        protected IContextBoundSettingsStore SettingsStore
        {
            get { return settingsStore; }
        }
    }
}