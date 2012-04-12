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

using System.Windows.Forms;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Features.Environment.Options.Inspections;
using JetBrains.UI.CommonControls.Fonts;
using JetBrains.UI.Options;
using JetBrains.UI.Options.Helpers;

namespace CleanCode.Settings
{
    /// <summary>
    /// Implements an options page that holds a set of setting editors stacked in lines from top to bottom.
    /// </summary>
    [OptionsPage(PID, "Clean Code", "CleanCode.Settings.CleanCode.png",
        ParentId = CodeInspectionPage.PID)]
    public class CleanCodeOptionsPage : AStackPanelOptionsPage
    {
        const string PID = "CleanCode";
        readonly Lifetime _lifetime;
        readonly OptionsSettingsSmartContext _settings;

        /// <summary>
        /// Creates new instance of CleanCodeOptionsPage
        /// </summary>
        public CleanCodeOptionsPage(Lifetime lifetime, FontsManager fontsManager,
                                                OptionsSettingsSmartContext settings)
            : base(lifetime, fontsManager, PID)
        {
            _lifetime = lifetime;
            _settings = settings;
            InitControls();
        }

        void InitControls()
        {
            Controls.Spin maximumDependencies; // This variable may be reused if there's more than one spin on the page
            Controls.HorzStackPanel stack;

            // The upper cue banner, stacked in the first line of our page, docked to full width with word wrapping, as needed
            Controls.Add(new Controls.Label(Stringtable.Options_Header));

            Controls.Add(new Controls.Label(Stringtable.Options_SubHeader));

            // Some spacing
            Controls.Add(JetBrains.UI.Options.Helpers.Controls.Separator.DefaultHeight);

            // A horizontal stack of a text label and a spin-edit
            Controls.Add(stack = new Controls.HorzStackPanel());
            stack.Controls.Add(new Controls.Label(Stringtable.Options_LabelMaximumDependenciesCheck));
            var maximumDependenciesCheckbox = new Controls.CheckBox();
            stack.Controls.Add(maximumDependenciesCheckbox);
            stack.Controls.Add(new Controls.Label(Stringtable.Options_LabelTooManyDependencies)); // The first column of the stack
            stack.Controls.Add(maximumDependencies = new Controls.Spin());

            // Set up the spin we've just added
            maximumDependencies.Maximum = new decimal(new[] {20, 0, 0, 0});
            maximumDependencies.Minimum = new decimal(new[] {1, 0, 0, 0});
            maximumDependencies.Value = new decimal(new[] {1, 0, 0, 0});

            _settings.SetBinding(_lifetime, (CleanCodeSettings s) => s.MaximumDependencies,
                                  maximumDependencies.IntegerValue);
            _settings.SetBinding(_lifetime, (CleanCodeSettings s) => s.MaximumDependenciesEnabled,
                                 maximumDependenciesCheckbox.Checked);
            maximumDependenciesCheckbox.CheckedChanged +=
                (sender, args) =>
                { maximumDependencies.Enabled = maximumDependenciesCheckbox.CheckState == CheckState.Checked; };



        }
    }
}