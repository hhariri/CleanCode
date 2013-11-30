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
using CleanCode.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

namespace CleanCode.TooManyDependencies
{
    public class TooManyDependenciesElementProcessor : IRecursiveElementProcessor
    {
        private readonly IDaemonProcess daemonProcess;
        private int MaxParams { get; set; }

        public TooManyDependenciesElementProcessor(IDaemonProcess daemonProcess, int maxParams)
        {
            Highlightings = new List<HighlightingInfo>();
            this.daemonProcess = daemonProcess;
            this.MaxParams = maxParams;
        }

        public List<HighlightingInfo> Highlightings { get; set; }

        private void ProcessFunctionDeclaration(IConstructorDeclaration constructorDeclaration)
        {
            var constructorParams = constructorDeclaration.ParameterDeclarations;

            var interfaceCount = constructorParams.Count(regularParameterDeclaration => regularParameterDeclaration.DeclaredElement.Type.IsInterfaceType());

            if (interfaceCount > MaxParams)
            {
                var message = StringTable.Warning_TooManyDependencies;
                var warning = new TooManyDependenciesHighlighting(message);
                Highlightings.Add(new HighlightingInfo(constructorDeclaration.GetNameDocumentRange(), warning));
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
            var constructorDeclaration = element as IConstructorDeclaration;
            if (constructorDeclaration != null)
                ProcessFunctionDeclaration(constructorDeclaration);
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