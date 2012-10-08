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
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;

namespace CleanCode.TooManyDependencies
{
    /// <summary>
    /// Daemon stage for comlexity analysis. This class is automatically loaded by ReSharper daemon 
    /// because it's marked with the attribute.
    /// </summary>
    [DaemonStage]
    public class TooManyDependenciesDaemonStage : IDaemonStage
    {

        /// <summary>
        /// This method provides a <see cref="IDaemonStageProcess"/> instance which is assigned to highlighting a single document
        /// </summary>
        public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings,
                                                              DaemonProcessKind kind)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (settings.GetValue((CleanCodeSettings s) => s.MaximumDependenciesEnabled))
            {
                return new[]
                    {
                        new TooManyDependenciesDaemonStageProcess(process,
                                                                  settings.GetValue(
                                                                      (CleanCodeSettings s) => s.MaximumDependencies))
                    };
            }
            return EmptyList<IDaemonStageProcess>.InstanceList;
        }

        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
        {
            // We want to add markers to the right-side stripe as well as contribute to document errors
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }
    }
}