using Compiler.CodeAnalysis.Binding;

namespace Compiler.CodeAnalysis;

internal sealed class Evaluator
{
    private readonly BoundExpression _root;
    private readonly Dictionary<VariableSymbol, object> _variables;

    public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables)
    {
        _root = root;
        _variables = variables;
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

        if (node is BoundVariableExpression v)
            return _variables[v.Variable];
        if (node is BoundAssignmentExpression a)
        {
            var value = EvalueteExpression(a.Expression);
            _variables[a.Variable] = value;
            return value;
        }

        if (node is BoundUnaryExpression u)
        {
            var operand = EvalueteExpression(u.Operand);
            if (u.Op.Kind == BoundUnaryOperatorKind.Identity)
                return (int)operand;
            else if (u.Op.Kind == BoundUnaryOperatorKind.Negation)
                return -(int)operand;
            else if (u.Op.Kind == BoundUnaryOperatorKind.LogicalNegation)
                return !(bool)operand;
            else
                throw new Exception($"Unexpected binary operator {u.Op.Kind}");
        }

        if (node is BoundBinaryExpression b)
        {
            var left = EvalueteExpression(b.Left);
            var right = EvalueteExpression(b.Right);
            switch (b.Op.Kind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return (int)left + (int)right;
                case BoundBinaryOperatorKind.Substraction:
                    return (int)left - (int)right;
                case BoundBinaryOperatorKind.Multiplication:
                    return (int)left * (int)right;
                case BoundBinaryOperatorKind.Division:
                    return (int)left / (int)right;
                case BoundBinaryOperatorKind.LogicalAnd:
                    return (bool)left && (bool)right;
                case BoundBinaryOperatorKind.LogicalOr:
                    return (bool)left || (bool)right;
                case BoundBinaryOperatorKind.Equals:
                    return Equals(left, right);
                case BoundBinaryOperatorKind.NotEquals:
                    return !Equals(left, right);
                default:
                    throw new Exception($"Unexpected binary operator {b.Op.Kind}");
            }
        }


        throw new Exception($"Unexpected Node {node.Kind}");
    }
}