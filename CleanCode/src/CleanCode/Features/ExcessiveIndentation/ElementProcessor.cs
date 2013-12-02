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
using System.Collections.Generic;
using System.Diagnostics;
using CleanCode.Resources;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework.Resources;

namespace CleanCode.Features.ExcessiveIndentation
{
    public class ElementProcessor : IRecursiveElementProcessor
    {
        private readonly List<HighlightingInfo> highlights = new List<HighlightingInfo>();

        private readonly IDaemonProcess daemonProcess;
        private readonly int maxDepth;

        public ElementProcessor(IDaemonProcess daemonProcess, int maxDepth)
        {
            this.daemonProcess = daemonProcess;
            this.maxDepth = maxDepth;
        }

        public List<HighlightingInfo> Highlightings
        {
            get
            {
                return highlights;
            }
        }

        private void ProcessMethod(IMethodDeclaration method)
        {
            var depth = method.GetChildrenDepth();
            Trace.WriteLine(string.Format("Method {0}, Depth={1}", method.NameIdentifier.Name, depth));

            if (depth > maxDepth)
            {
                var message = string.Format(Common.Warning_ExcessiveDepth);
                var highlighting = new Highlighting(message);
                Highlightings.Add(new HighlightingInfo(method.GetNameDocumentRange(), highlighting));
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
            var methodDeclaration = element as IMethodDeclaration;
            if (methodDeclaration != null)
            {
                ProcessMethod(methodDeclaration);
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