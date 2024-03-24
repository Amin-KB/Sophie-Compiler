namespace Sophie_Compiler.LexerAndParser;

public class Evaluator
{
    private readonly ExpressionSyntax _root;
    public Evaluator(ExpressionSyntax root)
    {
        _root = root;
    }

    public int Evaluete()
    {
        return EvalueteExpression(_root);
    }
    public int EvalueteExpression(ExpressionSyntax node)
    {
        if (node is NumberExpressionSyntax n)
        {
            return (int)n.NumberToken.Value;
        }
        if (node is BinaryExpressionSyntax b)
        {
            var left = EvalueteExpression(b.Left);
            var right = EvalueteExpression(b.Right);
            if (b.OperatorToken.SyntaxKind == SyntaxKind.PlusToken)
                return left + right;
            else if (b.OperatorToken.SyntaxKind == SyntaxKind.MinusToken)
                return left- right;
            else if (b.OperatorToken.SyntaxKind == SyntaxKind.StarToken)
                return left * right;
            else if (b.OperatorToken.SyntaxKind == SyntaxKind.SlashToken)
                return left / right;
            else
                throw new Exception($"Unexpected binary operator {b.OperatorToken.SyntaxKind}");
        }

        if (node is ParaenthesizedExpressionSyntax p)
            return EvalueteExpression(p.Expression);
        throw new Exception($"Unexpected Node {node.SyntaxKind}");
    }
}