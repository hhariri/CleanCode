using CleanCode.Features;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode
{
    public class MethodTooLongFeature
    {
        private readonly CleanCodeSettings settings;
        
        public MethodTooLongFeature(int)
        {
            this.settings = settings;
            this.cleanCodeDaemonStageProcess = cleanCodeDaemonStageProcess;
        }

        public void CheckMethodTooLong(IMethodDeclaration methodDeclaration, IHighlightingConsumer context)
        {
            var maxLength = settings.MaximumMethodLines;

            var statementCount = methodDeclaration.CountChildren<IStatement>();
            if (statementCount > maxLength)
            {
                var highlighting = new Features.MethodTooLong.Highlighting(Common.Warning_MethodTooLong);
                context.AddHighlighting(highlighting, methodDeclaration.GetNameDocumentRange());
            }
        }
    }
}