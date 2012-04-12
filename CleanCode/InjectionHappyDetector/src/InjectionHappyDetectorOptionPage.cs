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

using InjectionHappyDetector.resources;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Features.Environment.Options.Inspections;
using JetBrains.UI.CommonControls.Fonts;
using JetBrains.UI.Options;
using JetBrains.UI.Options.Helpers;

namespace InjectionHappyDetector
{
    /// <summary>
    /// Implements an options page that holds a set of setting editors stacked in lines from top to bottom.
    /// </summary>
    [OptionsPage(PID, "InjectionHappy Detector", "InjectionHappyDetector.resources.InjectionHappyDetector.png",
        ParentId = CodeInspectionPage.PID)]
    public class InjectionHappyDetectorOptionPage : AStackPanelOptionsPage
    {
        const string PID = "InjectionHappyDetector";
        readonly Lifetime _lifetime;
        readonly OptionsSettingsSmartContext _settings;

        /// <summary>
        /// Creates new instance of InjectionHappyDetectorOptionPage
        /// </summary>
        public InjectionHappyDetectorOptionPage(Lifetime lifetime, FontsManager fontsManager,
                                                OptionsSettingsSmartContext settings)
            : base(lifetime, fontsManager, PID)
        {
            _lifetime = lifetime;
            _settings = settings;
            InitControls();
        }

        void InitControls()
        {
            Controls.Spin spin; // This variable may be reused if there's more than one spin on the page
            Controls.HorzStackPanel stack;

            // The upper cue banner, stacked in the first line of our page, docked to full width with word wrapping, as needed
            Controls.Add(new Controls.Label(Stringtable.Options_Banner));

            // Some spacing
            Controls.Add(JetBrains.UI.Options.Helpers.Controls.Separator.DefaultHeight);

            // A horizontal stack of a text label and a spin-edit
            Controls.Add(stack = new Controls.HorzStackPanel());
            stack.Controls.Add(new Controls.Label(Stringtable.Options_MaximumArgumentLabel)); // The first column of the stack
            stack.Controls.Add(spin = new Controls.Spin());

            // Set up the spin we've just added
            spin.Maximum = new decimal(new[] {20, 0, 0, 0});
            spin.Minimum = new decimal(new[] {1, 0, 0, 0});
            spin.Value = new decimal(new[] {1, 0, 0, 0});

            // This binding will take the initial value from InjectionHappyDetectorAnalysisElementProcessor, put it into the edit, and pass back from UI to the control if the OK button is hit
            _settings.SetBinding(_lifetime, (InjectionHappyDetectorSettings s) => s.MaximumParameters,
                                  spin.IntegerValue);
        }
    }
}