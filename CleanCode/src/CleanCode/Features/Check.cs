using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
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
            get { return settingsStore; }
        }

        public void ExecuteIfEnabled(TElement methodDeclaration, IHighlightingConsumer context)
        {
            if (!IsEnabled)
            {
                return;
            }

            ExecuteCore(methodDeclaration, context);
        }

        protected abstract void ExecuteCore(TElement constructorDeclaration, IHighlightingConsumer consumer);
    }
}