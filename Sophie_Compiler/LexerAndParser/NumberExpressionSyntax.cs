namespace Sophie_Compiler.LexerAndParser;

public sealed class NumberExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.NumberExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NumberToken;
    }

    public SyntaxToken NumberToken { get;  }
    public NumberExpressionSyntax(SyntaxToken numberToken)
    {
        NumberToken = numberToken;
    }
}