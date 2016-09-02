using System;
using System.Collections.Generic;
using System.Linq;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using IContextBoundSettingsStore = JetBrains.Application.Settings.IContextBoundSettingsStore;

namespace CleanCode.Features.HollowNames
{
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
            var identifier = typeExpression.NameIdentifier;
            var documentRange = identifier.GetDocumentRange();
            var highlighting = new Highlighting(string.Format(Warnings.HollowTypeName, bannedSuffix), documentRange);
            consumer.AddHighlighting(highlighting);
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.HollowTypeNameEnabled); }
        }

        protected override string Value
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.HollowTypeNameString); }
        }
    }
}