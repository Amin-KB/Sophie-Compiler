using Sophie_Compiler.LexerAndParser.Binding;

namespace Sophie_Compiler.LexerAndParser;

internal sealed class Evaluator
{
    private readonly BoundExpression _root;

    public Evaluator(BoundExpression root)
    {
        _root = root;
    }

    public int Evaluete()
    {
        return EvalueteExpression(_root);
    }

    public int EvalueteExpression(BoundExpression node)
    {
        if (node is BoundLiteralExpression n)
        {
            return (int)n.Value;
        }

        if (node is BoundUnaryExpression u)
        {
            var operand = EvalueteExpression(u.Operand);
            if (u.OperatorKind== BoundUnaryOperatorKind.Identity)
                return operand;
            else if (u.OperatorKind == BoundUnaryOperatorKind.Negation)
                return -operand;
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
                    return left + right;
                case BoundBinaryOperatorKind.Substraction:
                    return left - right;
                case BoundBinaryOperatorKind.Multiplication:
                    return left * right;
                case BoundBinaryOperatorKind.Division:
                    return left / right;
                default:
                    throw new Exception($"Unexpected binary operator {b.OperatorKind}");
            }
         
        }

     
        throw new Exception($"Unexpected Node {node.Kind}");
    }
}