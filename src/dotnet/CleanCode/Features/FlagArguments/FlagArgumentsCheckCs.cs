using System.Collections.Generic;
using System.Linq;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

namespace CleanCode.Features.FlagArguments
{
    [ElementProblemAnalyzer(typeof(IMethodDeclaration),
        HighlightingTypes = new[]
        {
            typeof(FlagArgumentsHighlighting)
        })]
    public class FlagArgumentsCheckCs : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data,
            IHighlightingConsumer consumer)
        {
            var isFlagAnalysisEnabled = data.SettingsStore.GetValue((CleanCodeSettings s) => s.IsFlagAnalysisEnabled);
            if (!isFlagAnalysisEnabled) return;

            var parameterDeclarations = element.ParameterDeclarations.Where(parameterDeclaration =>
                IsFlagArgument(parameterDeclaration, element.Body));

            foreach (var parameterDeclaration in parameterDeclarations)
                AddHighlighting(consumer, parameterDeclaration);
        }

        private static bool IsFlagArgument(ITypeOwnerDeclaration typeOwnerDeclaration, ITreeNode node)
        {
            return IsOfTypeThatCanBeUsedAsFlag(typeOwnerDeclaration) &&
                   GetReferencesTo(typeOwnerDeclaration.DeclaredElement, node).Any();
        }

        private static bool IsOfTypeThatCanBeUsedAsFlag(ITypeOwnerDeclaration arg)
        {
            var type = arg.Type;
            return type.IsBool() || type.IsEnumType();
        }

        private static IEnumerable<IReferenceExpression> GetReferencesTo(IDeclaredElement declaredElement,
            ITreeNode body)
        {
            var ifStatements = body.GetChildrenRecursive<IIfStatement>();
            var allConditions = ifStatements.Select(statement => statement.Condition);
            var allReferencesInConditions = allConditions.SelectMany(expression =>
                expression.GetFlattenedHierarchyOfType<IReferenceExpression>());

            return GetReferencesToArgument(allReferencesInConditions, declaredElement);
        }

        private static IEnumerable<IReferenceExpression> GetReferencesToArgument(
            IEnumerable<IReferenceExpression> allReferencesInConditions, IDeclaredElement declaredElementInArgument)
        {
            return allReferencesInConditions.Where(reference =>
                IsReferenceToArgument(reference, declaredElementInArgument));
        }

        private static bool IsReferenceToArgument(IReferenceExpression referenceExpression, IDeclaredElement toFind)
        {
            if (referenceExpression == null)
            {
                return false;
            }

            var resolveResultWithInfo = referenceExpression.Reference.GetResolveResult();
            var declaredElement = resolveResultWithInfo.DeclaredElement;

            return declaredElement != null && declaredElement.ShortName == toFind.ShortName;
        }

        private static void AddHighlighting(IHighlightingConsumer consumer,
            ICSharpParameterDeclaration parameterDeclaration)
        {
            var documentRange = parameterDeclaration.GetDocumentRange();
            var highlighting = new FlagArgumentsHighlighting(documentRange);
            consumer.AddHighlighting(highlighting);
        }
    }
}