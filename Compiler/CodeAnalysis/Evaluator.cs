using Compiler.CodeAnalysis.Binding;
using Compiler.CodeAnalysis.Symbols;

namespace Compiler.CodeAnalysis;

internal sealed class Evaluator
{
    private readonly BoundBlockStatement _root;
    private readonly Dictionary<VariableSymbol, object> _variables;
    private object _lastValue;

    public Evaluator(BoundBlockStatement root, Dictionary<VariableSymbol, object> variables)
    {
        _root = root;
        _variables = variables;
    }

    public object Evaluate()
    {
        var labelToIndex = new Dictionary<BoundLabel, int>();

        for (int i = 0; i < _root.Statements.Length; i++)
        {
            if (_root.Statements[i] is BoundLabelStatement l)
                labelToIndex.Add(l.Label, i + 1);
        }

        var index = 0;
        while (index < _root.Statements.Length)
        {
            var statement = _root.Statements[index];
            switch (statement.Kind)
            {
                case BoundNodeKind.VariableDeclaration:
                    EvaluateVariableDeclaration((BoundVariableDeclaration)statement);
                    index++;
                    break;
                case BoundNodeKind.ExpressionStatement:
                    EvaluateExpressionStatement((BoundExpressionStatement)statement);
                    index++;
                    break;
                case BoundNodeKind.GotoStatement:
                    var gotoStatement = (BoundGotoStatement)statement;
                    index = labelToIndex[gotoStatement.Label];
                    break;
                case BoundNodeKind.ConditionalGotoStatement:
                    var conditionalGotoStatement = (BoundConditionalGotoStatement)statement;
                    var condition = (bool)EvaluateExpression(conditionalGotoStatement.Condition);
                    if (condition && !conditionalGotoStatement.JumpIfTrue ||
                        !condition && conditionalGotoStatement.JumpIfTrue)
                        index = labelToIndex[conditionalGotoStatement.Label];
                    else
                        index++;
                    break;
                case BoundNodeKind.LabelStatement:
                    index++;
                    break;
                default:
                    throw new Exception($"Unexpected Node {statement.Kind}");
            }
        }

        return _lastValue;
    }


    private void EvaluateVariableDeclaration(BoundVariableDeclaration node)
    {
        var value = EvaluateExpression(node.Initializer);
        _variables[node.Variable] = value;
        _lastValue = value;
    }

    private void EvaluateExpressionStatement(BoundExpressionStatement node)
    {
        _lastValue = EvaluateExpression(node.Expression);
    }


    private object EvaluateExpression(BoundExpression node)
    {
        switch (node.Kind)
        {
            case BoundNodeKind.LiteralExpression:
                return EvaluateLiteralExpression((BoundLiteralExpression)node);
            case BoundNodeKind.VariableExpression:
                return EvaluateVariableExpression((BoundVariableExpression)node);
            case BoundNodeKind.AssignmentExpression:
                return EvaluateAssignmentExpression((BoundAssignmentExpression)node);
            case BoundNodeKind.UnaryExpression:
                return EvaluateUnaryExpression((BoundUnaryExpression)node);
            case BoundNodeKind.BinaryExpression:
                return EvaluateBinaryExpression((BoundBinaryExpression)node);
            default:
                throw new Exception($"Unexpected Node {node.Kind}");
        }
    }

    private object EvaluateBinaryExpression(BoundBinaryExpression b)
    {
        var left = EvaluateExpression(b.Left);
        var right = EvaluateExpression(b.Right);
        switch (b.Op.Kind)
        {
            case BoundBinaryOperatorKind.Addition:
                return (int)left + (int)right;
            case BoundBinaryOperatorKind.Subtraction:
                return (int)left - (int)right;
            case BoundBinaryOperatorKind.Multiplication:
                return (int)left * (int)right;

            case BoundBinaryOperatorKind.BitwiseAnd:
                if (b.Type == TypeSymbol.Int)
                    return (int)left & (int)right;
                else
                    return (bool)left & (bool)right;
            case BoundBinaryOperatorKind.BitwiseOr:
                if (b.Type == TypeSymbol.Int)
                    return (int)left | (int)right;
                else
                    return (bool)left | (bool)right;
            case BoundBinaryOperatorKind.BitwiseXor:
                if (b.Type == TypeSymbol.Int)
                    return (int)left ^ (int)right;
                else
                    return (bool)left ^ (bool)right;


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
            case BoundBinaryOperatorKind.Less:
                return (int)left < (int)right;
            case BoundBinaryOperatorKind.LessOrEquals:
                return (int)left <= (int)right;
            case BoundBinaryOperatorKind.Greater:
                return (int)left > (int)right;
            case BoundBinaryOperatorKind.GreaterOrEquals:
                return (int)left >= (int)right;
            default:
                throw new Exception($"Unexpected binary operator {b.Op.Kind}");
        }
    }

    private object EvaluateUnaryExpression(BoundUnaryExpression u)
    {
        var operand = EvaluateExpression(u.Operand);
        if (u.Op.Kind == BoundUnaryOperatorKind.Identity)
            return (int)operand;
        else if (u.Op.Kind == BoundUnaryOperatorKind.Negation)
            return -(int)operand;
        else if (u.Op.Kind == BoundUnaryOperatorKind.LogicalNegation)
            return !(bool)operand;
        else if (u.Op.Kind == BoundUnaryOperatorKind.OnesComplement)
            return ~ (int)operand;
        else
            throw new Exception($"Unexpected binary operator {u.Op.Kind}");
    }

    private object EvaluateAssignmentExpression(BoundAssignmentExpression a)
    {
        var value = EvaluateExpression(a.Expression);
        _variables[a.Variable] = value;
        return value;
    }

    private object EvaluateVariableExpression(BoundVariableExpression v)
    {
        return _variables[v.Variable];
    }

    private static object EvaluateLiteralExpression(BoundLiteralExpression n)
    {
        return n.Value;
    }
}