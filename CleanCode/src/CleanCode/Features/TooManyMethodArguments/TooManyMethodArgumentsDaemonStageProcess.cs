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
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace CleanCode.Features.TooManyMethodArguments
{
    public class TooManyMethodArgumentsDaemonStageProcess : CSharpDaemonStageProcessBase
    {
        private readonly IDaemonProcess daemonProcess;
        private readonly int maxParams;

        public TooManyMethodArgumentsDaemonStageProcess(IDaemonProcess daemonProcess, ICSharpFile file, int maxParams)
            : base(daemonProcess, file)
        {
            this.daemonProcess = daemonProcess;
            this.maxParams = maxParams;
        }

        public override void Execute(Action<DaemonStageResult> commiter)
        {
            // Running visitor against the PSI
            var elementProcessor = new TooManyMethodArgumentsElementProcessor(daemonProcess, maxParams);
            File.ProcessDescendants(elementProcessor);

            // Checking if the daemon is interrupted by user activity
            if (daemonProcess.InterruptFlag)
                throw new ProcessCancelledException();

            // Commit the result into document
            commiter(new DaemonStageResult(elementProcessor.Highlightings));
        }
    }
}