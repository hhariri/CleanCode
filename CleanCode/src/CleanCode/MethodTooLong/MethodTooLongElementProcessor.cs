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
using System.Diagnostics;
using System.Linq;
using CleanCode.Resources;
using CleanCode.Settings;
using CleanCode.TooManyDependencies;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.UnitTestFramework.Resources;

namespace CleanCode.MethodTooLong
{
    public class MethodTooLongElementProcessor : IRecursiveElementProcessor
    {
        private readonly List<HighlightingInfo> _highlights = new List<HighlightingInfo>();

        private readonly IDaemonProcess _daemonProcess;
        private readonly int _maxParams;

        public MethodTooLongElementProcessor(IDaemonProcess daemonProcess, int maxParams)
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

        private void ProcessMethodDeclaration(IMethodDeclaration method)
        {
            var lines = CountLines(method);
            if (lines > 5)
            {
                var message = string.Format(StringTable.Warning_MethodTooLong, lines);
                var warning = new MethodTooLongHighlighting(message);
                Highlightings.Add(new HighlightingInfo(method.GetNameDocumentRange(), warning));
            }
        }

        private int CountLines(IMethodDeclaration method)
        {
            var totalLines = 0;
            foreach (var treeNode in method.Children())
            {
                if (ContainsLines(treeNode))
                {
                    totalLines += CountLines(treeNode);
                }
            }


            return totalLines;
        }

        private int CountLines(ITreeNode node)
        {
            var treeNodes = node.Children().ToList();

            var statements = treeNodes.OfType<IStatement>();
            var count = statements.Count();


            var ifStatements = treeNodes.OfType<IIfStatement>();

            foreach (var ifStatement in ifStatements)
            {
                count += CountLines(ifStatement);
            } 

            var blocks = node.Children().OfType<IBlock>();

            foreach (var block in blocks)
            {
                count += CountLines(block);
            }            

            Debug.WriteLine(count);
            return count;
        }

        private bool ContainsLines(ITreeNode treeNode)
        {
            return true;
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
                ProcessMethodDeclaration(methodDeclaration);
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