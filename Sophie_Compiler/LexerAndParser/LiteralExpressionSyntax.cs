namespace Sophie_Compiler.LexerAndParser;

public sealed class LiteralExpressionSyntax:ExpressionSyntax
{
    public override SyntaxKind SyntaxKind => SyntaxKind.LiteralExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return LiteralToken;
    }

    public SyntaxToken LiteralToken { get;  }
    public object Value { get;  }

    public LiteralExpressionSyntax(SyntaxToken literalToken):this(literalToken,literalToken.Value)
    {
      
    }
    public LiteralExpressionSyntax(SyntaxToken literalToken,object value)
    {
        LiteralToken = literalToken;
        Value = value;
    }
}