using System;
using CleanCode.Features.ComplexExpression;
using CleanCode.Features.ExcessiveIndentation;
using CleanCode.Features.FlagArguments;
using CleanCode.Features.HollowNames;
using CleanCode.Features.MethodNameNotMeaningful;
using CleanCode.Features.MethodTooLong;
using CleanCode.Features.TooManyDeclarations;
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
        private readonly MethodTooLongCheck methodTooLongCheck;
        private readonly TooManyMethodArgumentsCheck tooManyArgumentsCheck;
        private readonly ExcessiveIndentationCheck excessiveIndentationCheck;
        private readonly TooManyDependenciesCheck tooManyDependenciesCheck;
        private readonly MethodNameNotMeaningfulCheck methodNamesNotMeaningfulCheck;
        private readonly FlagArgumentsCheck flagArgumentsCheck;
        private readonly ComplexExpressionCheck complexExpressionCheck;
        private readonly HollowNamesCheck hollowNamesCheck;
        private readonly TooManyDeclarationsCheck tooManyDeclarationsCheck;

        public CleanCodeDaemonStageProcess(IDaemonProcess daemonProcess, ICSharpFile file, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, file)
        {
            this.settingsStore = settingsStore;

            // TODO: This is starting to feel like a beach of Benidorm in July. Refactoring needed.
            methodTooLongCheck = new MethodTooLongCheck(settingsStore);
            tooManyArgumentsCheck = new TooManyMethodArgumentsCheck(settingsStore);
            excessiveIndentationCheck = new ExcessiveIndentationCheck(settingsStore);
            tooManyDependenciesCheck = new TooManyDependenciesCheck(settingsStore);
            methodNamesNotMeaningfulCheck = new MethodNameNotMeaningfulCheck(settingsStore);
            flagArgumentsCheck = new FlagArgumentsCheck(settingsStore);
            complexExpressionCheck = new ComplexExpressionCheck(settingsStore);
            hollowNamesCheck = new HollowNamesCheck(settingsStore);
            tooManyDeclarationsCheck = new TooManyDeclarationsCheck(settingsStore);
        }

        public override void Execute(Action<DaemonStageResult> commiter)
        {
            HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter, settingsStore);
        }

        public override void VisitMethodDeclaration(IMethodDeclaration methodDeclaration, IHighlightingConsumer context)
        {
            methodTooLongCheck.ExecuteIfEnabled(methodDeclaration, context);
            tooManyArgumentsCheck.ExecuteIfEnabled(methodDeclaration, context);
            excessiveIndentationCheck.ExecuteIfEnabled(methodDeclaration, context);
            methodNamesNotMeaningfulCheck.ExecuteIfEnabled(methodDeclaration, context);
            flagArgumentsCheck.ExecuteIfEnabled(methodDeclaration, context);         
            tooManyDeclarationsCheck.ExecuteIfEnabled(methodDeclaration, context);
        }

        public override void VisitConstructorDeclaration(IConstructorDeclaration constructorDeclaration, IHighlightingConsumer context)
        {
            tooManyDependenciesCheck.ExecuteIfEnabled(constructorDeclaration, context);
        }

        public override void VisitClassDeclaration(IClassDeclaration classDeclaration, IHighlightingConsumer context)
        {
            hollowNamesCheck.ExecuteIfEnabled(classDeclaration, context);
        }

        public override void VisitIfStatement(IIfStatement ifStatementParam, IHighlightingConsumer context)
        {
            complexExpressionCheck.ExecuteIfEnabled(ifStatementParam.Condition, context);
        }

        public override void VisitWhileStatement(IWhileStatement whileStatementParam, IHighlightingConsumer context)
        {
            complexExpressionCheck.ExecuteIfEnabled(whileStatementParam.Condition, context);
        }

        public override void VisitForStatement(IForStatement forStatementParam, IHighlightingConsumer context)
        {
            complexExpressionCheck.ExecuteIfEnabled(forStatementParam.Condition, context);
        }

        public override void VisitDoStatement(IDoStatement doStatementParam, IHighlightingConsumer context)
        {
            complexExpressionCheck.ExecuteIfEnabled(doStatementParam.Condition, context);
        }
    }
}