using System.Linq;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ComplexExpression
{
    // TODO: This might be better working with IOperatorExpression
    // Walk up from the operator to the highest containing expression
    // and count depth?
    [ElementProblemAnalyzer(typeof(IIfStatement),
        typeof(ILoopWithConditionStatement),
        typeof(IConditionalTernaryExpression),
        typeof(IAssignmentExpression),
        typeof(IExpressionInitializer),
        HighlightingTypes = new[]
        {
            typeof(ComplexConditionExpressionHighlighting)
        })]
    public class ComplexConditionExpressionCheck : ElementProblemAnalyzer<ICSharpTreeNode>
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
            // Covers for, while and do
            var loopStatement = node as ILoopWithConditionStatement;
            if (loopStatement != null)
                return loopStatement.Condition;

            var ifStatement = node as IIfStatement;
            if (ifStatement != null)
                return ifStatement.Condition;

            var conditionalTernaryExpression = node as IConditionalTernaryExpression;
            if (conditionalTernaryExpression != null)
                return conditionalTernaryExpression.ConditionOperand;

            // TODO: Should these two be part of this check?
            // Perhaps they should also check the type of the resulting
            // variable to be bool?
            var assignmentExpression = node as IAssignmentExpression;
            if (assignmentExpression != null)
                return assignmentExpression.Source;

            var expressionInitializer = node as IExpressionInitializer;
            return expressionInitializer?.Value;
        }

        private static void CheckExpression(IExpression expression, ElementProblemAnalyzerData data,
            IHighlightingConsumer consumer)
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