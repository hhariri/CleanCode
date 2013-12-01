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
using System.Linq;
using CleanCode.Resources;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.TooManyMethodArguments
{
    public class TooManyMethodArgumentsElementProcessor : IRecursiveElementProcessor
    {
        private readonly List<HighlightingInfo> _highlights = new List<HighlightingInfo>();

        private readonly IDaemonProcess _daemonProcess;
        private readonly int _maxParams;

        public TooManyMethodArgumentsElementProcessor(IDaemonProcess daemonProcess, int maxParams)
        {
            _daemonProcess = daemonProcess;
            _maxParams = maxParams;
        }

        public List<HighlightingInfo> Highlightings
        {
            get
            {
                return _highlights;
            }
        }

        private void ProcessFunctionDeclaration(IMethodDeclaration methodDeclaration)
        {
            var constructorParams = methodDeclaration.ParameterDeclarations;

            var paramCount = constructorParams.Count();

           if (paramCount > _maxParams)
            {
                string message = Common.Warning_TooManyMethodArguments;
                var warning = new TooManyMethodArgumentsHighlighting(message);
                _highlights.Add(new HighlightingInfo(methodDeclaration.GetNameDocumentRange(), warning));
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
                ProcessFunctionDeclaration(methodDeclaration);
            }
        }

        public bool ProcessingIsFinished
        {
            get
            {
                return _daemonProcess.InterruptFlag;
            }
        }
    }
}