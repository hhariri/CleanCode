using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features
{
    public abstract class Check<TElement>
        where TElement : ITreeNode
    {
        private readonly IContextBoundSettingsStore settingsStore;

        protected Check(IContextBoundSettingsStore settingsStore)
        {
            this.settingsStore = settingsStore;
        }

        protected abstract bool IsEnabled { get; }

        protected IContextBoundSettingsStore SettingsStore
        {
            get { return this.settingsStore; }
        }

        public void ExecuteIfEnabled(TElement methodDeclaration, IHighlightingConsumer context)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            this.ExecuteCore(methodDeclaration, context);
        }

        protected abstract void ExecuteCore(TElement classDeclaration, IHighlightingConsumer consumer);
    }
}