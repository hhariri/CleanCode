using CleanCode.Resources.Icons82;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Features.Environment.Options.Inspections;
using JetBrains.UI.Application;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;

namespace CleanCode.Settings
{
    [OptionsPage(PageId, "Clean Code", typeof(SettingsThemedIcons.CleanCode), ParentId = CodeInspectionPage.PID)]
    public partial class OptionsPage : IOptionsPage
    {
        private Lifetime lifetime;
        private OptionsSettingsSmartContext settings;
        const string PageId = "CleanCode";

        public OptionsPage(Lifetime lifetime, IUIApplication environment,
            OptionsSettingsSmartContext settings)
            : this()
        {
            DataContext = new OptionsViewModel(settings);
        }

        private OptionsPage()
        {
            InitializeComponent();
        }

        public bool OnOk()
        {
            return true;
        }

        public bool ValidatePage()
        {
            return true;
        }

        public EitherControl Control
        {
            get
            {
                return this;
            }
        }

        public string Id { get; private set; }
    }
}
