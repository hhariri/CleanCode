using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using System.Collections.Generic;
using System.Linq;
using CleanCode.Features.ChainedReferences;
using JetBrains.ReSharper.Psi;


namespace CleanCode.Features.TooManyChainedReferences
{
    using JetBrains.DocumentModel;

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
            
            var count = reference.CountChildren<IReferenceExpression>();

            if (count >= Threshold)
            {
                var rootType = GetReturnTypeFrom(reference.Reference);

                if (IsStillUnknown(rootType))
                {
                    return;
                }

                var childrenTypes = this.GetTypesFromChildren(reference);

                if (!EveryTypeIsTheSame(rootType, childrenTypes))
                {
                    AddHighlightning(reference, consumer);
                }              
            }
        }

        private IEnumerable<IType> GetTypesFromChildren(IReferenceExpression reference)
        {
            var children = reference.GetChildrenRecursive<IReferenceExpression>();

            var typesFromChildren = children.Select(expression => this.GetReturnTypeFrom(expression.Reference));
            return typesFromChildren;
        }

        private static void AddHighlightning(IReferenceExpression reference, IHighlightingConsumer consumer)
        {
            var highlighting = new Highlighting(Warnings.ChainedReferences);
            var nameIdentifier = reference.NameIdentifier;
            consumer.AddHighlighting(highlighting, nameIdentifier.GetDocumentRange());
        }

        private static bool EveryTypeIsTheSame(IType type, IEnumerable<IType> typesFromChildren)
        {
            return typesFromChildren.Any(otherType => Equals(type, otherType));
        }

        private static bool IsStillUnknown(IType type)
        {
            return type == null;
        }

        private IType GetReturnTypeFrom(IReferenceExpressionReference reference)
        {
            if (reference.CurrentResolveResult != null)
            {
                var declaredElement = reference.CurrentResolveResult.DeclaredElement;
                var parameternsOwner = declaredElement as IParametersOwner;
                if (parameternsOwner != null)
                {
                    var r = parameternsOwner.ReturnType;
                    return r;
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