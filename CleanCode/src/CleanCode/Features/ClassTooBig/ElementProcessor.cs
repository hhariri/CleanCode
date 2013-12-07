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

using System.Collections.Generic;
using CleanCode.Resources;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ClassTooBig
{
    public class ElementProcessor : IRecursiveElementProcessor
    {
        private readonly List<HighlightingInfo> highlights = new List<HighlightingInfo>();

        private readonly IDaemonProcess daemonProcess;
        private readonly int maxMethods;

        public ElementProcessor(IDaemonProcess daemonProcess, int maxMethods)
        {
            this.daemonProcess = daemonProcess;
            this.maxMethods = maxMethods;
        }

        public List<HighlightingInfo> Highlightings
        {
            get
            {
                return highlights;
            }
        }

        private void ProcessClass(IClassDeclaration classDeclaration)
        {
            var depth = classDeclaration.CountChildren<IMethodDeclaration>();
            
            if (depth > maxMethods)
            {
                var message = string.Format(Common.Warning_ClassTooBig);
                var highlighting = new Highlighting(message);
                Highlightings.Add(new HighlightingInfo(classDeclaration.GetNameDocumentRange(), highlighting));
            }
        }


        public bool InteriorShouldBeProcessed(ITreeNode element)
        {
            return true;
        }

        public void ProcessBeforeInterior(ITreeNode element)
        {
        }

        public void ProcessAfterInterior(ITreeNode element)
        {
            var methodDeclaration = element as IClassDeclaration;
            if (methodDeclaration != null)
            {
                ProcessClass(methodDeclaration);
            }
        }

        public bool ProcessingIsFinished
        {
            get
            {
                return daemonProcess.InterruptFlag;
            }
        }
    }
}