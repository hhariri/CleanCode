using System;
using CleanCode.Features.TooManyDependencies;
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
        private readonly TooManyDependenciesCheck tooManyDependenciesCheck;

        public CleanCodeDaemonStageProcess(IDaemonProcess daemonProcess, ICSharpFile file, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, file)
        {
            this.settingsStore = settingsStore;

            // TODO: This is starting to feel like a beach of Benidorm in July. Refactoring needed.
            tooManyArgumentsCheck = new TooManyMethodArgumentsCheck(settingsStore);
            tooManyDependenciesCheck = new TooManyDependenciesCheck(settingsStore);
        }

        public override void Execute(Action<DaemonStageResult> commiter)
        {
            HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter, settingsStore);
        }

        public override void VisitMethodDeclaration(IMethodDeclaration methodDeclaration, IHighlightingConsumer context)
        {
            tooManyArgumentsCheck.ExecuteIfEnabled(methodDeclaration, context);
        }

        public override void VisitConstructorDeclaration(IConstructorDeclaration constructorDeclaration, IHighlightingConsumer context)
        {
            tooManyDependenciesCheck.ExecuteIfEnabled(constructorDeclaration, context);
        }
    }
}