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

using JetBrains.Application.Settings;
using JetBrains.ReSharper.Settings;

namespace CleanCode.Settings
{
    [SettingsKey(typeof(CodeInspectionSettings), "CleanCode")]
    public class CleanCodeSettings
    {
        [SettingsEntry(3, "TooManyDependenciesMaximum")]
        public readonly int TooManyDependenciesMaximum;
        [SettingsEntry(true, "TooManyDependenciesMaximumEnabled")]
        public readonly bool TooManyDependenciesMaximumEnabled;

        [SettingsEntry(3, "TooManyMethodArgumentsMaximum")]
        public readonly int TooManyMethodArgumentsMaximum;
        [SettingsEntry(true, "TooManyMethodArgumentsEnabled")]
        public readonly bool TooManyMethodArgumentsEnabled;

        [SettingsEntry(true, "MethodTooLongEnabled")]
        public readonly bool MethodTooLongEnabled;
        [SettingsEntry(15, "MethodTooLongMaximum")]
        public readonly int MethodTooLongMaximum;

        [SettingsEntry(3, "ExcessiveIndentationMaximum")]
        public int ExcessiveIndentationMaximum { get; set; }
        [SettingsEntry(true, "ExcessiveIndentationEnabled")]
        public bool ExcessiveIndentationEnabled { get; set; }

        [SettingsEntry(true, "ClassTooBigEnabled")]
        public bool ClassTooBigEnabled { get; set; }
        [SettingsEntry(20, "ClassTooBigMaximum")]
        public int ClassTooBigMaximum { get; set; }

        [SettingsEntry(true, "TooManyChainedReferencesEnabled")]
        public bool TooManyChainedReferencesEnabled { get; set; }
        [SettingsEntry(2, "TooManyChainedReferencesMaximum")]
        public int TooManyChainedReferencesMaximum { get; set; }

        [SettingsEntry(true, "MethodNameNotMeaningfulMinimumEnabled")]
        public bool MethodNameNotMeaningfulMinimumEnabled { get; set; }
        [SettingsEntry(4, "MethodNameNotMeaningfulMinimum")]
        public int MethodNameNotMeaningfulMinimum { get; set; }

        [SettingsEntry(true, "FlagArgumentsEnabled")]
        public bool FlagArgumentsEnabled { get; set; }

        [SettingsEntry(true, "HollowTypeNameEnabled")]
        public bool HollowTypeNameEnabled { get; set; }  

        [SettingsEntry(true, "ComplexExpressionEnabled")]
        public bool ComplexExpressionEnabled { get; set; }
        [SettingsEntry(1, "ComplexExpressionMaximum")]
        public int ComplexExpressionMaximum { get; set; }
    }
}