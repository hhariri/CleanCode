using System.Linq;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Psi.Tree;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.ReSharper.Psi.Util;
using IType = JetBrains.ReSharper.Psi.IType;


namespace CleanCode.Features.FlagArguments
{
    public class FlagArgumentsCheck : SimpleCheck<IMethodDeclaration, int>
    {
        public FlagArgumentsCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IMethodDeclaration statement, IHighlightingConsumer consumer)
        {
            var parameterDeclarations = statement.ParameterDeclarations;

            foreach (var parameterDeclaration in parameterDeclarations)
            {
                if (IsFlagArgument(parameterDeclaration, statement.Body))
                {
                    AddHighlighting(consumer, parameterDeclaration);
                }
            }
        }

        private static void AddHighlighting(IHighlightingConsumer consumer, ICSharpParameterDeclaration parameterDeclaration)
        {
            var highlighting = new Highlighting(Warnings.FlagArgument);
            consumer.AddHighlighting(highlighting, parameterDeclaration.GetDocumentRange());
        }

        private static bool IsFlagArgument(ITypeOwnerDeclaration typeOwnerDeclaration, ITreeNode node)
        {
            if (IsOfTypeThatCanBeUsedAsFlag(typeOwnerDeclaration))
            {
                var references = GetReferencesTo(typeOwnerDeclaration.DeclaredElement, node);
                return references.Any();    
            }

            return false;
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

            var allReferencesInConditions =
                allConditions.SelectMany(expression => expression.GetFlattenedHierarchyOfType<IReferenceExpression>());

            return GetReferencesToArgument(allReferencesInConditions, declaredElement);
        }

        private static IEnumerable<IReferenceExpression> GetReferencesToArgument(IEnumerable<IReferenceExpression> allReferencesInConditions, IDeclaredElement declaredElementInArgument)
        {
            return allReferencesInConditions.Where(reference => IsReferenceToArgument(reference, declaredElementInArgument));
        }

        private static bool IsReferenceToArgument(IReferenceExpression r, IDeclaredElement toFind)
        {
            var resolveResultWithInfo = r.Reference.GetResolveResult();
            var declaredElement = resolveResultWithInfo.DeclaredElement;

            Debug.Assert(declaredElement != null, "declaredElement != null");

            return declaredElement.ShortName == toFind.ShortName;
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyMethodArgumentsMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyMethodArgumentsEnabled); }
        }
    }
}