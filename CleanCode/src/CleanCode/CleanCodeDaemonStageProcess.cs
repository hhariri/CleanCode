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
using System.Linq;
using CleanCode.Features;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

namespace CleanCode
{
    public class CleanCodeDaemonStageProcess : CSharpDaemonStageProcessBase
    {
        private readonly IDaemonProcess daemonProcess;
        private readonly IContextBoundSettingsStore settingsStore;

        public CleanCodeDaemonStageProcess(IDaemonProcess daemonProcess, ICSharpFile file, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, file)
        {
            this.daemonProcess = daemonProcess;

            this.settingsStore = settingsStore;
        }

        public override void Execute(Action<DaemonStageResult> commiter)
        {
            HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter, settingsStore);

            // Checking if the daemon is interrupted by user activity
            if (daemonProcess.InterruptFlag)
            {
                throw new ProcessCancelledException();
            }
        }

        public override void VisitMethodDeclaration(IMethodDeclaration method, IHighlightingConsumer context)
        {
            CheckMethodTooLong(method, context);
            CheckTooManyArguments(method, context);
        }

        public override void VisitConstructorDeclaration(IConstructorDeclaration constructorDeclarationParam, IHighlightingConsumer context)
        {
            CheckTooManyDependencies(constructorDeclarationParam, context);
        }

        private void CheckTooManyDependencies(IConstructorDeclaration constructorDeclarationParam, IHighlightingConsumer context)
        {
            var maxDependencies = settingsStore.GetValue((CleanCodeSettings s) => s.MaximumDependencies);

            var depedencies = constructorDeclarationParam.ParameterDeclarations.Select(
                declaration => declaration.DeclaredElement != null &&
                               declaration.DeclaredElement.Type.IsInterfaceType());

            var dependenciesCount = depedencies.Count();

            if (dependenciesCount > maxDependencies)
            {
                var highlighting = new Features.TooManyDependencies.Highlighting(Common.Warning_TooManyDependencies);
                context.AddHighlighting(highlighting, constructorDeclarationParam.GetNameDocumentRange());
            }
        }


        private void CheckTooManyArguments(IMethodDeclaration methodDeclaration, IHighlightingConsumer context)
        {
            var parameterDeclarations = methodDeclaration.ParameterDeclarations;
            var maxParameters = settingsStore.GetValue((CleanCodeSettings s) => s.MaximumMethodArguments);

            if (parameterDeclarations.Count > maxParameters)
            {
                var highlighting = new Features.TooManyMethodArguments.Highlighting(Common.Warning_TooManyMethodArguments);
                context.AddHighlighting(highlighting, methodDeclaration.GetNameDocumentRange());
            }
        }

        private void CheckMethodTooLong(IMethodDeclaration methodDeclaration, IHighlightingConsumer context)
        {
            var maxLength = settingsStore.GetValue((CleanCodeSettings s) => s.MaximumMethodLines);

            var statementCount = methodDeclaration.CountChildren<IStatement>();
            if (statementCount > maxLength)
            {
                var highlighting = new Features.MethodTooLong.Highlighting(Common.Warning_MethodTooLong);
                context.AddHighlighting(highlighting, methodDeclaration.GetNameDocumentRange());
            }
        }

        //#region Used by ChainedReferences (Refactor as class)

        //public override void ProcessAfterInterior(ITreeNode element, IHighlightingConsumer consumer)
        //{
        //    //CheckChainedReferences(element, consumer);
        //}

        //private void CheckChainedReferences(ITreeNode element, IHighlightingConsumer consumer)
        //{
        //    var reference = element as IReferenceExpression;

        //    if (reference != null && !ParentIsReference(element))
        //    {
        //        ProcessReference(reference, consumer);
        //    }
        //}

        //private void ProcessReference(IReferenceExpression reference, IHighlightingConsumer consumer)
        //{
        //    var length = reference.CountChildren<IReferenceExpression>();
        //    var maximumChainedReferences = settingsStore.GetValue((CleanCodeSettings s) => s.MaximumChainedReferences);

        //    if (length > maximumChainedReferences)
        //    {
        //        var highlighting = new Features.ChainedReferences.Highlighting(Common.Warning_ChainedReferences);
        //        consumer.AddHighlighting(highlighting, reference.GetDocumentRange());
        //    }
        //}

        //private bool ParentIsReference(ITreeNode element)
        //{
        //    var reference = element.Parent as IReferenceExpression;
        //    return reference != null;
        //}

        //#endregion

    }
}