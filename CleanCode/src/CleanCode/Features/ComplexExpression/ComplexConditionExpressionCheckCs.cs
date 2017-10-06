using System.Linq;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ComplexExpression
{
    [ElementProblemAnalyzer(typeof(IIfStatement),
        typeof(ILoopWithConditionStatement),
        typeof(IConditionalTernaryExpression),
        typeof(IAssignmentExpression),
        typeof(IExpressionInitializer),
        HighlightingTypes = new[]
        {
            typeof(ComplexConditionExpressionHighlighting)
        })]
    public class ComplexConditionExpressionCheckCs : ElementProblemAnalyzer<ICSharpTreeNode>
    {
        protected override void Run(ICSharpTreeNode element, ElementProblemAnalyzerData data,
            IHighlightingConsumer consumer)
        {
            var expression = GetExpression(element);
            if (expression != null)
                CheckExpression(expression, data, consumer);
        }

        private static IExpression GetExpression(ITreeNode node)
        {
            switch (node)
            {
                case ILoopWithConditionStatement loopWithConditionStatement: return loopWithConditionStatement.Condition;
                case IIfStatement ifStatement: return ifStatement.Condition;
                case IConditionalTernaryExpression conditionalTernaryExpression: return conditionalTernaryExpression.ConditionOperand;
                case IAssignmentExpression assignmentExpression: return assignmentExpression.Source;
                case IExpressionInitializer expressionInitializer: return expressionInitializer.Value;
                default:
                    return null;
            }
        }

        private static void CheckExpression(IExpression expression, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var maxExpressions = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumExpressionsInCondition);
            var expressionCount = expression.GetChildrenRecursive<IOperatorExpression>().Count();

            if (expressionCount > maxExpressions)
            {
                var documentRange = expression.GetDocumentRange();
                var highlighting = new ComplexConditionExpressionHighlighting(documentRange);
                consumer.AddHighlighting(highlighting);
            }
        }
    }
}