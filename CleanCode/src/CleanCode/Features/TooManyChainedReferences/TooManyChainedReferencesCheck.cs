using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;


namespace CleanCode.Features.TooManyChainedReferences
{
    using JetBrains.ReSharper.Psi.Resolve;

    public class TooManyChainedReferencesCheck : SimpleCheck<IReferenceExpression, int>
    {
        public TooManyChainedReferencesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IReferenceExpression referenceExpression, IHighlightingConsumer consumer)
        {
            if (referenceExpression != null)
            {
                ProcessReference(referenceExpression, consumer);
            }
        }

        private void ProcessReference(IReferenceExpression reference, IHighlightingConsumer consumer)
        {
            var chainedReferencesCount = reference.CountChildren<IReferenceExpression>();

            if (chainedReferencesCount >= this.Threshold)
            {
                var rootType = GetRootType(reference);

                if (IsTypeStillUnknown(rootType))
                {
                    return;
                }

                if (this.RootTypeAndChainedReferencesTypesAreDifferent(reference, rootType))
                {
                    AddHighlightning(reference, consumer);
                }
            }
        }

        private static IType GetRootType(IReferenceExpression reference)
        {
            var referenceOfReference = reference.Reference;
            referenceOfReference.Resolve();

            var rootType = GetReturnTypeFrom(referenceOfReference);
            return rootType;
        }

        private bool RootTypeAndChainedReferencesTypesAreDifferent(IReferenceExpression reference, IType rootType)
        {
            var childrenTypes = this.GetTypesFromChildren(reference);
            return SomeTypeIsDifferent(rootType, childrenTypes);
        }

        private IEnumerable<IType> GetTypesFromChildren(IReferenceExpression reference)
        {
            var children = reference.GetChildrenRecursive<IReferenceExpression>().Where(expression => expression.Reference.IsQualified);
            var typesFromChildren = children.Select(expression => GetReturnTypeFrom(expression.Reference));
            return typesFromChildren;
        }

        private static void AddHighlightning(IReferenceExpression reference, IHighlightingConsumer consumer)
        {
            var highlighting = new Highlighting(Warnings.ChainedReferences);
            var nameIdentifier = reference.NameIdentifier;
            consumer.AddHighlighting(highlighting, nameIdentifier.GetDocumentRange());
        }

        private static bool SomeTypeIsDifferent(IType type, IEnumerable<IType> typesFromChildren)
        {
            bool someTypeIsDifferent = typesFromChildren.Any(otherType => Equals(type, otherType));
            return someTypeIsDifferent;
        }

        private static bool Equals(IType type, IType otherType)
        {
            return !type.ToString().Equals(otherType.ToString());
        }

        private static bool IsTypeStillUnknown(IType type)
        {
            return type == null;
        }

        private static IType GetReturnTypeFrom(IReference reference)
        {
            reference.Resolve();

            if (reference.CurrentResolveResult != null)
            {
                var declaredElement = reference.CurrentResolveResult.DeclaredElement;
                var parameternsOwner = declaredElement as IParametersOwner;
                if (parameternsOwner != null)
                {
                    return parameternsOwner.ReturnType;
                }               
            }

            return null;
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyChainedReferencesMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyChainedReferencesEnabled); }
        }
    }
}