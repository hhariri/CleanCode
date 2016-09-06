using System;
using CleanCode.Features.TooManyMethodArguments;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace CleanCode
{
    public class CleanCodeDaemonStageProcess : CSharpDaemonStageProcessBase
    {
        private readonly IContextBoundSettingsStore settingsStore;
        private readonly TooManyMethodArgumentsCheck tooManyArgumentsCheck;

        public CleanCodeDaemonStageProcess(IDaemonProcess daemonProcess, ICSharpFile file, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, file)
        {
            this.settingsStore = settingsStore;

            // TODO: This is starting to feel like a beach of Benidorm in July. Refactoring needed.
            tooManyArgumentsCheck = new TooManyMethodArgumentsCheck(settingsStore);
        }

        public override void Execute(Action<DaemonStageResult> commiter)
        {
            HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter, settingsStore);
        }

        public override void VisitMethodDeclaration(IMethodDeclaration methodDeclaration, IHighlightingConsumer context)
        {
            tooManyArgumentsCheck.ExecuteIfEnabled(methodDeclaration, context);
        }
    }
}