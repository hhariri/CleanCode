using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Xaml.Tree.MarkupExtensions;

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

    public class HollowNamesCheck : MonoValueCheck<IClassDeclaration, string>
    {
        private const string Separator = ",";

        public HollowNamesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IClassDeclaration constructorDeclaration, IHighlightingConsumer consumer)
        {
            var suffixes = GetSuffixes();

            var match = GetFirstMatchOrDefault(constructorDeclaration.DeclaredName, suffixes);

            if (match != null)
            {
                AddHightlightning(match, consumer, constructorDeclaration);
            }
        }

        private IEnumerable<string> GetSuffixes()
        {
            return Value.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string GetFirstMatchOrDefault(string declaredName, IEnumerable<string> suffixes)
        {
            return suffixes.FirstOrDefault(declaredName.EndsWith);
        }

        private void AddHightlightning(string bannedSuffix, IHighlightingConsumer consumer, IClassDeclaration typeExpression)
        {
            var highlighting = new Highlighting(string.Format(Warnings.HollowTypeName, bannedSuffix));
            var identifier = typeExpression.NameIdentifier;
            consumer.AddHighlighting(highlighting, identifier.GetDocumentRange());
        }

        protected override bool IsEnabled
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.HollowTypeNameEnabled); }
        }

        protected override string Value
        {
            get { return this.SettingsStore.GetValue((CleanCodeSettings s) => s.HollowTypeNameString); }
        }
    }
}