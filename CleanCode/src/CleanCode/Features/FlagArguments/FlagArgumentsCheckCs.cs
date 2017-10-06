using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
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
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var parameterDeclarations = element.ParameterDeclarations;

            foreach (var parameterDeclaration in parameterDeclarations)
            {
                if (IsFlagArgument(parameterDeclaration, element.Body))
                {
                    AddHighlighting(consumer, parameterDeclaration);
                }
            }
        }

        private static bool IsFlagArgument(ITypeOwnerDeclaration typeOwnerDeclaration, ITreeNode node)
        {
            return IsOfTypeThatCanBeUsedAsFlag(typeOwnerDeclaration) && GetReferencesTo(typeOwnerDeclaration.DeclaredElement, node).Any();
        }

        private static bool IsOfTypeThatCanBeUsedAsFlag(ITypeOwnerDeclaration arg)
        {
            var type = arg.Type;
            return type.IsBool() || type.IsEnumType();
        }

        private static IEnumerable<IReferenceExpression> GetReferencesTo(IDeclaredElement declaredElement, ITreeNode body)
        {
            var ifStatements = body.GetChildrenRecursive<IIfStatement>();
            var allConditions = ifStatements.Select(statement => statement.Condition);
            var allReferencesInConditions = allConditions.SelectMany(expression => expression.GetFlattenedHierarchyOfType<IReferenceExpression>());

            return GetReferencesToArgument(allReferencesInConditions, declaredElement);
        }

        private static IEnumerable<IReferenceExpression> GetReferencesToArgument(IEnumerable<IReferenceExpression> allReferencesInConditions, IDeclaredElement declaredElementInArgument)
        {
            return allReferencesInConditions.Where(reference => IsReferenceToArgument(reference, declaredElementInArgument));
        }

        private static bool IsReferenceToArgument(IReferenceExpression referenceExpression, IDeclaredElement toFind)
        {
            if (referenceExpression == null)
            {
                return false;
            }

            var resolveResultWithInfo = referenceExpression.Reference.GetResolveResult();
            var declaredElement = resolveResultWithInfo.DeclaredElement;

            Debug.Assert(declaredElement != null, "declaredElement != null");

            return declaredElement.ShortName == toFind.ShortName;
        }

        private static void AddHighlighting(IHighlightingConsumer consumer, ICSharpParameterDeclaration parameterDeclaration)
        {
            var documentRange = parameterDeclaration.GetDocumentRange();
            var highlighting = new FlagArgumentsHighlighting(documentRange);
            consumer.AddHighlighting(highlighting);
        }
    }
}