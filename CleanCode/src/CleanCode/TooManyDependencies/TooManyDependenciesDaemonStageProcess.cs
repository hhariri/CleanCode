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
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace CleanCode.TooManyDependencies
{
  public class TooManyDependenciesDaemonStageProcess : IDaemonStageProcess
  {
    private readonly IDaemonProcess _daemonProcess;
    private readonly int _maxParams;

    public TooManyDependenciesDaemonStageProcess(IDaemonProcess daemonProcess, int maxParams)
    {
      _daemonProcess = daemonProcess;
      _maxParams = maxParams;
    }

    public void Execute(Action<DaemonStageResult> commiter)
    {
      // Getting PSI (AST) for the file being highlighted
      PsiManager manager = _daemonProcess.Solution.GetPsiServices().PsiManager;

      var file = manager.GetPsiFile(_daemonProcess.SourceFile, CSharpLanguage.Instance) as ICSharpFile;
      if (file == null)
        return;

      // Running visitor against the PSI
      var elementProcessor = new TooManyDependenciesElementProcessor(_daemonProcess, _maxParams);
      file.ProcessDescendants(elementProcessor);

      // Checking if the daemon is interrupted by user activity
      if (_daemonProcess.InterruptFlag)
        throw new ProcessCancelledException();

      // Commit the result into document
      commiter(new DaemonStageResult(elementProcessor.Highlightings));
    }

    public IDaemonProcess DaemonProcess
    {
      get { return _daemonProcess; }
    }
  }
}