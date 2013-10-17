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
using System.Reflection;
using System.Runtime.InteropServices;
using CleanCode.TooManyDependencies;
using JetBrains.Application.PluginSupport;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Asp.Highlightings;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly : AssemblyTitle("ReSharper Clean Code Plugin")]
[assembly : AssemblyDescription("")]
[assembly : AssemblyCompany("Hadi Hariri and Contributors")]
[assembly : AssemblyProduct("ReSharper Clean Code Plugin")]
[assembly : AssemblyCopyright("Copyright \u00A9 2012 Hadi Hariri and Contributors")]
[assembly : AssemblyTrademark("")]
[assembly : AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly : ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly : Guid("97927FF9-8C9C-4DC5-A309-29C23F41DA47")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly : AssemblyVersion("3.0.0.0")]
[assembly : AssemblyFileVersion("3.0.0.0")]

[assembly : PluginTitle("ReSharper Clean Code Plugin")]
[assembly : PluginVendor("Hadi Hariri and Contributors")]

[assembly:
  RegisterConfigurableSeverity(
        TooManyDependenciesHighlighting.SeverityID,
        null,
        HighlightingGroupIds.CodeSmell,
        "Too Many Dependencies",
        "Too many dependencies passed into constructor",
        Severity.SUGGESTION,
        false)]