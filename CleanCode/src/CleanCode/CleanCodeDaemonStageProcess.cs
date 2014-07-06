#region License
// Copyright (C) 2012 Hadi Hariri and Contributors
// 
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software 
// without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons 
// to whom the Software is furnished to do so, subject to the 
// following conditions:
//  
// The above copyright notice and this permission notice shall 
// be included in all copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using CleanCode.Features.ChainedReferences;
using CleanCode.Features.ClassTooBig;
using CleanCode.Features.ExcessiveIndentation;
using CleanCode.Features.FlagArguments;
using CleanCode.Features.MethodNameNotMeaningful;
using CleanCode.Features.MethodTooLong;
using CleanCode.Features.TooManyDeclarations;
using CleanCode.Features.TooManyDependencies;
using CleanCode.Features.TooManyMethodArguments;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace CleanCode
{
    using CleanCode.Features.ComplexExpression;
    using CleanCode.Features.HollowNames;

    public class CleanCodeDaemonStageProcess : CSharpDaemonStageProcessBase
    {
        private readonly IDaemonProcess daemonProcess;
        private readonly IContextBoundSettingsStore settingsStore;

        private readonly MethodTooLongCheck methodTooLongCheck;
        private readonly ClassTooBigCheck classTooBigCheck;
        private readonly TooManyMethodArgumentsCheck tooManyArgumentsCheck;
        private readonly ExcessiveIndentationCheck excessiveIndentationCheck;
        private readonly TooManyDependenciesCheck tooManyDependenciesCheck;
        private readonly MethodNameNotMeaningfulCheck methodNamesNotMeaningfulCheck;
        private readonly ChainedReferencesCheck chainedReferencesCheck;
        private readonly FlagArgumentsCheck flagArgumentsCheck;
        private readonly ComplexExpressionCheck complexExpressionCheck;
        private readonly HollowNamesCheck hollowNamesCheck;
        private readonly TooManyDeclarationsCheck tooManyDeclarationsCheck;

        public CleanCodeDaemonStageProcess(IDaemonProcess daemonProcess, ICSharpFile file, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, file)
        {
            this.daemonProcess = daemonProcess;
            this.settingsStore = settingsStore;

            // TODO: This is starting to feel like a beach of Benidorm in July. Refactoring needed.
            methodTooLongCheck = new MethodTooLongCheck(settingsStore);
            classTooBigCheck = new ClassTooBigCheck(settingsStore);
            tooManyArgumentsCheck = new TooManyMethodArgumentsCheck(settingsStore);
            excessiveIndentationCheck = new ExcessiveIndentationCheck(settingsStore);
            tooManyDependenciesCheck = new TooManyDependenciesCheck(settingsStore);
            methodNamesNotMeaningfulCheck = new MethodNameNotMeaningfulCheck(settingsStore);
            chainedReferencesCheck = new ChainedReferencesCheck(settingsStore);
            flagArgumentsCheck = new FlagArgumentsCheck(settingsStore);
            complexExpressionCheck = new ComplexExpressionCheck(settingsStore);
            hollowNamesCheck = new HollowNamesCheck(settingsStore);
            tooManyDeclarationsCheck = new TooManyDeclarationsCheck(settingsStore);
        }

        public override void Execute(Action<DaemonStageResult> commiter)
        {
            HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter, settingsStore);
            if (daemonProcess.InterruptFlag)
            {
                throw new ProcessCancelledException();
            }
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

        public override void VisitCSharpStatement(ICSharpStatement cSharpStatementParam, IHighlightingConsumer context)
        {
            chainedReferencesCheck.ExecuteIfEnabled(cSharpStatementParam, context);
        }

        public override void VisitConstructorDeclaration(IConstructorDeclaration constructorDeclaration, IHighlightingConsumer context)
        {
            tooManyDependenciesCheck.ExecuteIfEnabled(constructorDeclaration, context);
        }

        public override void VisitClassDeclaration(IClassDeclaration classDeclaration, IHighlightingConsumer context)
        {
            classTooBigCheck.ExecuteIfEnabled(classDeclaration, context);
            hollowNamesCheck.ExecuteIfEnabled(classDeclaration, context);
        }

        public override void VisitIfStatement(IIfStatement ifStatementParam, IHighlightingConsumer context)
        {
            this.complexExpressionCheck.ExecuteIfEnabled(ifStatementParam.Condition, context);
        }

        public override void VisitWhileStatement(IWhileStatement whileStatementParam, IHighlightingConsumer context)
        {
            this.complexExpressionCheck.ExecuteIfEnabled(whileStatementParam.Condition, context);
        }

        public override void VisitForStatement(IForStatement forStatementParam, IHighlightingConsumer context)
        {
            this.complexExpressionCheck.ExecuteIfEnabled(forStatementParam.Condition, context);
        }

        public override void VisitDoStatement(IDoStatement doStatementParam, IHighlightingConsumer context)
        {
            this.complexExpressionCheck.ExecuteIfEnabled(doStatementParam.Condition, context);
        }
    }
}