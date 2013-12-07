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
        [SettingsEntry(3, "MaximumDependencies")]
        public readonly int MaximumDependencies;
        [SettingsEntry(true, "MaximumDependenciesEnabled")]
        public readonly bool MaximumDependenciesEnabled;

        [SettingsEntry(3, "MaximumMethodArguments")]
        public readonly int MaximumMethodArguments;
        [SettingsEntry(true, "MaximumDependenciesEnabled")]
        public readonly bool MaximumMethodArgumentsEnabled;

        [SettingsEntry(true, "EnabledMethodTooLong")]
        public readonly bool MethodTooLongEnabled;
        [SettingsEntry(15, "MaximumMethodLines")]
        public readonly int MaximumMethodLines;

        [SettingsEntry(3, "MaximumCodeDepth")]
        public int MaximumCodeDepth { get; set; }
        [SettingsEntry(true, "MaximumCodeDepthEnabled")]
        public bool MaximumCodeDepthEnabled { get; set; }

        [SettingsEntry(true, "MaximumMethodsPerClassEnabled")]
        public bool MaximumMethodsPerClassEnabled { get; set; }
        [SettingsEntry(20, "MaximumMethodsPerClass")]
        public int MaximumMethodsPerClass { get; set; }

        [SettingsEntry(true, "MaximumChainedReferencesEnabled")]
        public bool MaximumChainedReferencesEnabled { get; set; }
        [SettingsEntry(2, "MaximumChainedReferences")]
        public int MaximumChainedReferences { get; set; }
    }
}