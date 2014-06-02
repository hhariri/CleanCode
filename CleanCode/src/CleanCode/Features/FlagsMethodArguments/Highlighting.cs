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

using CleanCode.Features.FlagsMethodArguments;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

[assembly: RegisterConfigurableSeverity(Highlighting.SeverityID, null, 
    HighlightingGroupIds.CodeSmell, "Flags Method Arguments",
    "This method does more than one thing. Please review it.",
    Severity.WARNING, false)]

namespace CleanCode.Features.FlagsMethodArguments
{
    /// <summary>
    /// The highlighting that warns about high complexity
    /// </summary>
    [ConfigurableSeverityHighlighting(SeverityID, CSharpLanguage.Name)]
    public class Highlighting : IHighlighting
    {
        internal const string SeverityID = "FlagsMethodArguments";
        private readonly string tooltip;

        public Highlighting(string toolTip)
        {
            tooltip = toolTip;
        }

        public string ToolTip
        {
            get { return tooltip; }
        }

        public string ErrorStripeToolTip
        {
            get { return tooltip; }
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}