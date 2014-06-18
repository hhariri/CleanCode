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
using CleanCode.Features.ClassTooBig;
using CleanCode.Features.ExcessiveIndentation;
using CleanCode.Features.MethodNameNotMeaningful;
using CleanCode.Features.MethodTooLong;
using CleanCode.Features.TooManyChainedReferences;
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
    public class CleanCodeDaemonStageProcess : CSharpDaemonStageProcessBase
    {
        private readonly IDaemonProcess daemonProcess;
        private readonly IContextBoundSettingsStore settingsStore;

        private readonly MethodTooLongCheck methodTooLongCheck;
        private readonly ClassTooBigCheck classTooBigCheck;
        private readonly TooManyMethodArgumentsCheck tooManyArgumentsCheck;
        private readonly ExcessiveIndentationCheck excessiveIndentationCheck;
        private readonly TooManyDependenciesCheck tooManyDependenciesCheck;
        private readonly MethodNamesNotMeaningfulCheck methodNamesNotMeaningfulCheck;
        private readonly TooManyChainedReferencesCheck tooManyChainedReferencesCheck;

        public CleanCodeDaemonStageProcess(IDaemonProcess daemonProcess, ICSharpFile file, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, file)
        {
            this.daemonProcess = daemonProcess;
            this.settingsStore = settingsStore;

            // Simple checks.
            methodTooLongCheck = new MethodTooLongCheck(settingsStore);
            classTooBigCheck = new ClassTooBigCheck(settingsStore);
            tooManyArgumentsCheck = new TooManyMethodArgumentsCheck(settingsStore);
            excessiveIndentationCheck = new ExcessiveIndentationCheck(settingsStore);
            tooManyDependenciesCheck = new TooManyDependenciesCheck(settingsStore);
            methodNamesNotMeaningfulCheck = new MethodNamesNotMeaningfulCheck(settingsStore);
            tooManyChainedReferencesCheck = new TooManyChainedReferencesCheck(settingsStore);
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
        }

        public override void VisitCSharpStatement(ICSharpStatement cSharpStatementParam, IHighlightingConsumer context)
        {
            tooManyChainedReferencesCheck.ExecuteIfEnabled(cSharpStatementParam, context);
        }

        public override void VisitConstructorDeclaration(IConstructorDeclaration constructorDeclaration, IHighlightingConsumer context)
        {
            tooManyDependenciesCheck.ExecuteIfEnabled(constructorDeclaration, context);
        }

        public override void VisitClassDeclaration(IClassDeclaration classDeclaration, IHighlightingConsumer context)
        {
            classTooBigCheck.ExecuteIfEnabled(classDeclaration, context);
        }
    }
}