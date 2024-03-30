namespace Sophie_Compiler.LexerAndParser;

public sealed class LiteralExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.LiteralExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return LiteralToken;
    }

    public SyntaxToken LiteralToken { get;  }
    public LiteralExpressionSyntax(SyntaxToken literlaToken)
    {
        LiteralToken = literlaToken;
    }
}