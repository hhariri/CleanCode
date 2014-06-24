namespace CleanCode.Features.HollowNames
{
    using System.Collections.Generic;
    using System.Linq;

    using CleanCode.Resources;
    using CleanCode.Settings;
    using JetBrains.Application.Settings;
    using JetBrains.ReSharper.Daemon.CSharp.Stages;
    using JetBrains.ReSharper.Daemon.Stages;
    using JetBrains.ReSharper.Psi.Tree;

    public class HollowNamesCheck : SimpleCheckBase<ITypeDeclaration>
    {
        public HollowNamesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(ITypeDeclaration typeExpression, IHighlightingConsumer consumer)
        {

            var suffixes = new[] { "Manager", "Controller", "Processor", "Helper", "Handler" };

            var match = GetFirstMatchOrDefault(typeExpression.DeclaredName, suffixes);

            if (match != null)
            {
                AddHightlightning(match, consumer, typeExpression);
            }
        }

        private static string GetFirstMatchOrDefault(string declaredName, IEnumerable<string> suffixes)
        {
            return suffixes.FirstOrDefault(declaredName.EndsWith);
        }

        private void AddHightlightning(string bannedSuffix, IHighlightingConsumer consumer, ITypeDeclaration typeExpression)
        {
            var highlighting = new Highlighting(string.Format(Warnings.HollowTypeName, bannedSuffix));
            consumer.AddHighlighting(highlighting, typeExpression.GetDocumentRange());
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.HollowTypeNameEnabled); }
        }
    }
}