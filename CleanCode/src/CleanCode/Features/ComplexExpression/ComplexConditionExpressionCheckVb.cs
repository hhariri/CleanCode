using CleanCode.Extension;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.VB.Tree;

namespace CleanCode.Features.ComplexExpression
{
    // TODO: This might be better working with IOperatorExpression
    // Walk up from the operator to the highest containing expression
    // and count depth?
    [ElementProblemAnalyzer(typeof(IBlockIfStatement),
        typeof(IElseIfStatement),
        typeof(IWhileStatement),
        typeof(IForEachStatement),
        typeof(IForStatement),
        typeof(IConditionalExpression),
        typeof(IExpressionStatement),
        typeof(ILineIfStatement),
        HighlightingTypes = new[]
        {
            typeof(ComplexConditionExpressionHighlighting)
        })]
    public class ComplexConditionExpressionCheckVb : ElementProblemAnalyzer<IVBTreeNode>
    {
        protected override void Run(IVBTreeNode element, ElementProblemAnalyzerData data,
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
                case IBlockIfStatement blockIfStatement: return blockIfStatement.Expression;
                case IElseIfStatement elseIfStatement: return elseIfStatement.Expression;
                case IWhileStatement whileStatement: return whileStatement.Expression;
                case IForEachStatement forEachStatement: return forEachStatement.Expression;
                case IForStatement forStatement: return forStatement.StepExpression;
                case IConditionalExpression conditionalExpression: return conditionalExpression.Condition;
                case IExpressionStatement expressionStatement: return expressionStatement.Expression;
                case ILineIfStatement lineIfStatement: return lineIfStatement.Expression;
                default:
                    return null;
            }
        }

        private static void CheckExpression(IExpression expression, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var maxExpressions = data.SettingsStore.GetValue((CleanCodeSettings s) => s.MaximumExpressionsInCondition);
            var expressionCount = GetExpressionCount(expression);

            if (expressionCount > maxExpressions)
            {
                var documentRange = expression.GetDocumentRange();
                var highlighting = new ComplexConditionExpressionHighlighting(documentRange);
                consumer.AddHighlighting(highlighting);
            }
        }

        private static int GetExpressionCount(IExpression expression)
        {
            return expression.GetExpressionCount<IRelationalExpression>() +
                expression.GetExpressionCount<ILogicalAndExpression>() +
                expression.GetExpressionCount<ILogicalOrExpression>();
        }
    }
}