using Sophie_Compiler.LexerAndParser.Binding;

namespace Sophie_Compiler.LexerAndParser;

internal sealed class Evaluator
{
    private readonly BoundExpression _root;

    public Evaluator(BoundExpression root)
    {
        _root = root;
    }

    public object Evaluate()
    {
        return EvalueteExpression(_root);
    }

    public object EvalueteExpression(BoundExpression node)
    {
        if (node is BoundLiteralExpression n)
        {
            return n.Value;
        }

        if (node is BoundUnaryExpression u)
        {
            var operand = EvalueteExpression(u.Operand);
            if (u.OperatorKind== BoundUnaryOperatorKind.Identity)
                return (int)operand;
            else if (u.OperatorKind == BoundUnaryOperatorKind.Negation)
                return -(int)operand;
            else if (u.OperatorKind == BoundUnaryOperatorKind.LogicalNegation)
                return !(bool)operand;
            else
                throw new Exception($"Unexpected binary operator {u.OperatorKind}");
        }

        if (node is BoundBinaryExpression b)
        {
            var left = EvalueteExpression(b.Left);
            var right = EvalueteExpression(b.Right);
            switch (b.OperatorKind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return (int)left + (int)right;
                case BoundBinaryOperatorKind.Substraction:
                    return (int)left -(int) right;
                case BoundBinaryOperatorKind.Multiplication:
                    return (int)left * (int)right;
                case BoundBinaryOperatorKind.Division:
                    return (int)left / (int)right;
                case BoundBinaryOperatorKind.LogicalAnd:
                    return (bool)left && (bool)right;
                case BoundBinaryOperatorKind.LogicalOr:
                    return (bool)left || (bool)right;
                default:
                    throw new Exception($"Unexpected binary operator {b.OperatorKind}");
            }
         
        }

     
        throw new Exception($"Unexpected Node {node.Kind}");
    }
}