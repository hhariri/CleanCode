using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features
{
    public abstract class SimpleCheckBase<TElement>
        where TElement : ITreeNode
    {
        private readonly IContextBoundSettingsStore settingsStore;

        protected SimpleCheckBase(IContextBoundSettingsStore settingsStore)
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

        protected abstract void ExecuteCore(TElement typeExpression, IHighlightingConsumer consumer);
    }

    public abstract class SimpleCheck<TElement, TThreshold> : SimpleCheckBase<TElement>
        where TElement : ITreeNode
    {
        protected SimpleCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected abstract TThreshold Threshold { get; }
    }
}